using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class PayrollRunService : GenericService<PayrollRun>, IPayrollRunService
    {
        private readonly IPayrollEntryService _entryService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public PayrollRunService(
            IUnitOfWork uow,
            IPayrollEntryService entryService,
            IEmailService emailService,
            IMapper mapper) : base(uow)
        {
            _entryService = entryService;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<(bool Success, int? PayrollRunId, string? ErrorMessage)> GeneratePayrollRunAsync(
            int year,
            int month)
        {
            try
            {
                // Validate period
                if (month < 1 || month > 12)
                    return (false, null, "Invalid month.");

                // Check if payroll run already exists
                if (await PayrollRunExistsAsync(year, month))
                    return (false, null, $"Payroll run for {new DateTime(year, month, 1):MMMM yyyy} already exists.");

                // Calculate period dates
                var periodStart = new DateOnly(year, month, 1);
                var periodEnd = periodStart.AddMonths(1).AddDays(-1);

                // Create payroll run (no currency needed)
                var payrollRun = new PayrollRun
                {
                    Name = $"{new DateTime(year, month, 1):MMMM yyyy} Payroll",
                    PeriodStart = periodStart,
                    PeriodEnd = periodEnd,
                    IsLocked = false,
                    CreatedDate = DateTime.Now
                };

                await _repository.AddAsync(payrollRun);
                await _uow.CompleteAsync();

                // Get all active employees
                var activeEmployees = await _uow.Employees
                    .GetAllAsIQueryable()
                    .Where(e => e.IsActive)
                    .Select(e => e.Id)
                    .ToListAsync();

                // Create payroll entry for each employee
                int successCount = 0;
                foreach (var employeeId in activeEmployees)
                {
                    var result = await _entryService.CreatePayrollEntryAsync(
                        payrollRun.Id,
                        employeeId,
                        year,
                        month);

                    if (result.Success)
                        successCount++;
                }

                // Calculate total amount
                await RecalculateTotalAmountAsync(payrollRun.Id);

                return (true, payrollRun.Id, $"Payroll run created successfully with {successCount} entries.");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error generating payroll run: {ex.Message}");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RegeneratePayrollRunAsync(int payrollRunId)
        {
            try
            {
                var payrollRun = await _repository
                    .GetAllAsIQueryable()
                    .Include(pr => pr.PayrollEntries)
                    .FirstOrDefaultAsync(pr => pr.Id == payrollRunId);

                if (payrollRun == null)
                    return (false, "Payroll run not found.");

                if (payrollRun.IsLocked)
                    return (false, "Cannot regenerate a locked payroll run. Please unlock it first.");

                // Delete existing entries
                foreach (var entry in payrollRun.PayrollEntries.ToList())
                {
                    _uow.PayrollEntries.Delete(entry.Id);
                }
                await _uow.CompleteAsync();

                // Regenerate entries
                var activeEmployees = await _uow.Employees
                    .GetAllAsIQueryable()
                    .Where(e => e.IsActive)
                    .Select(e => e.Id)
                    .ToListAsync();

                int successCount = 0;
                foreach (var employeeId in activeEmployees)
                {
                    var result = await _entryService.CreatePayrollEntryAsync(
                        payrollRunId,
                        employeeId,
                        payrollRun.PeriodStart.Year,
                        payrollRun.PeriodStart.Month);

                    if (result.Success)
                        successCount++;
                }

                // Recalculate total
                await RecalculateTotalAmountAsync(payrollRunId);

                return (true, $"Payroll run regenerated successfully with {successCount} entries.");
            }
            catch (Exception ex)
            {
                return (false, $"Error regenerating payroll run: {ex.Message}");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> LockPayrollRunAsync(int payrollRunId)
        {
            try
            {
                var payrollRun = await _repository
                    .GetAllAsIQueryable()
                    .Include(pr => pr.PayrollEntries)
                        .ThenInclude(pe => pe.Employee)
                    .Include(pr => pr.PayrollEntries)
                        .ThenInclude(pe => pe.Currency)
                    .FirstOrDefaultAsync(pr => pr.Id == payrollRunId);

                if (payrollRun == null)
                    return (false, "Payroll run not found.");

                if (payrollRun.IsLocked)
                    return (false, "Payroll run is already locked.");

                if (!payrollRun.PayrollEntries.Any())
                    return (false, "Cannot lock an empty payroll run.");

                // Lock the payroll run
                payrollRun.IsLocked = true;
                payrollRun.ProcessedDate = DateTime.Now;

                _repository.Update(payrollRun);
                await _uow.CompleteAsync();

                // Send email notifications to all employees
                await SendPayrollNotificationsAsync(payrollRun);

                return (true, "Payroll run confirmed and locked successfully. Email notifications sent to all employees.");
            }
            catch (Exception ex)
            {
                return (false, $"Error locking payroll run: {ex.Message}");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UnlockPayrollRunAsync(int payrollRunId)
        {
            try
            {
                var payrollRun = await _repository.GetByIdAsync(payrollRunId);

                if (payrollRun == null)
                    return (false, "Payroll run not found.");

                if (!payrollRun.IsLocked)
                    return (false, "Payroll run is not locked.");

                payrollRun.IsLocked = false;
                payrollRun.ProcessedDate = null;

                _repository.Update(payrollRun);
                await _uow.CompleteAsync();

                return (true, "Payroll run unlocked successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error unlocking payroll run: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PayrollRunVM>> GetAllPayrollRunsAsync(int? year = null, int? month = null, bool? isLocked = null)
        {
            var query = _repository
                .GetAllAsIQueryable()
                .Include(pr => pr.PayrollEntries)
                    .ThenInclude(pe => pe.Currency)
                .AsQueryable();

            if (year.HasValue)
                query = query.Where(pr => pr.PeriodStart.Year == year.Value);

            if (month.HasValue)
                query = query.Where(pr => pr.PeriodStart.Month == month.Value);

            if (isLocked.HasValue)
                query = query.Where(pr => pr.IsLocked == isLocked.Value);

            var runs = await query
                .OrderByDescending(pr => pr.PeriodStart)
                .ToListAsync();

            var vms = runs.Select(pr =>
            {
                var vm = _mapper.Map<PayrollRunVM>(pr);

                // Calculate currency totals
                vm.CurrencyTotals = pr.PayrollEntries
                    .GroupBy(pe => pe.Currency.Code)
                    .ToDictionary(g => g.Key, g => g.Sum(pe => pe.NetAmount));

                // Get list of currencies used
                vm.CurrenciesUsed = pr.PayrollEntries
                    .Select(pe => pe.Currency.Code)
                    .Distinct()
                    .ToList();

                return vm;
            }).ToList();

            return vms;
        }

        public async Task<PayrollRunDetailsVM?> GetPayrollRunDetailsAsync(int payrollRunId)
        {
            var payrollRun = await _repository
                .GetAllAsIQueryable()
                .Include(pr => pr.PayrollEntries)
                    .ThenInclude(pe => pe.Employee)
                        .ThenInclude(e => e.Department)
                .Include(pr => pr.PayrollEntries)
                    .ThenInclude(pe => pe.Employee)
                        .ThenInclude(e => e.JobTitle)
                .Include(pr => pr.PayrollEntries)
                    .ThenInclude(pe => pe.Currency)
                .FirstOrDefaultAsync(pr => pr.Id == payrollRunId);

            if (payrollRun == null)
                return null;

            var vm = _mapper.Map<PayrollRunDetailsVM>(payrollRun);

            // Calculate currency totals
            vm.CurrencyTotals = payrollRun.PayrollEntries
                .GroupBy(pe => pe.Currency.Code)
                .ToDictionary(g => g.Key, g => g.Sum(pe => pe.NetAmount));

            // Get list of currencies used
            vm.CurrenciesUsed = payrollRun.PayrollEntries
                .Select(pe => pe.Currency)
                .DistinctBy(c => c.Id)
                .ToList();

            return vm;
        }

        public async Task<(bool Success, string? ErrorMessage)> DeletePayrollRunAsync(int payrollRunId)
        {
            try
            {
                var payrollRun = await _repository.GetByIdAsync(payrollRunId);

                if (payrollRun == null)
                    return (false, "Payroll run not found.");

                if (payrollRun.IsLocked)
                    return (false, "Cannot delete a locked payroll run. Please unlock it first.");

                _repository.Delete(payrollRunId);
                await _uow.CompleteAsync();

                return (true, "Payroll run deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting payroll run: {ex.Message}");
            }
        }

        public async Task<bool> PayrollRunExistsAsync(int year, int month)
        {
            var periodStart = new DateOnly(year, month, 1);
            return await _repository
                .GetAllAsIQueryable()
                .AnyAsync(pr => pr.PeriodStart.Year == year && pr.PeriodStart.Month == month);
        }

        public async Task<PayrollRun?> GetPayrollRunByPeriodAsync(int year, int month)
        {
            var periodStart = new DateOnly(year, month, 1);
            return await _repository
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(pr => pr.PeriodStart.Year == year && pr.PeriodStart.Month == month);
        }

        // ========== PRIVATE HELPER METHODS ==========

        private async Task RecalculateTotalAmountAsync(int payrollRunId)
        {
            var payrollRun = await _repository
                .GetAllAsIQueryable()
                .Include(pr => pr.PayrollEntries)
                .FirstOrDefaultAsync(pr => pr.Id == payrollRunId);

            if (payrollRun == null) return;

            // Sum all net amounts (note: this may mix currencies, consider removing TotalAmount field)
            // For now, just sum all entries regardless of currency
            payrollRun.TotalAmount = payrollRun.PayrollEntries.Sum(pe => pe.NetAmount);

            _repository.Update(payrollRun);
            await _uow.CompleteAsync();
        }

        private async Task SendPayrollNotificationsAsync(PayrollRun payrollRun)
        {
            foreach (var entry in payrollRun.PayrollEntries)
            {
                var employee = entry.Employee;
                if (string.IsNullOrEmpty(employee.WorkEmail))
                    continue;

                var subject = $"Payroll Notification - {payrollRun.Name}";
                var body = GeneratePayrollEmailBody(payrollRun, entry, employee);

                try
                {
                    await _emailService.SendEmailAsync(employee.WorkEmail, subject, body);
                }
                catch (Exception ex)
                {
                    // Log error but continue processing
                    Console.WriteLine($"Failed to send email to {employee.WorkEmail}: {ex.Message}");
                }
            }
        }

        private string GeneratePayrollEmailBody(PayrollRun payrollRun, PayrollEntry entry, Employee employee)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #ddd; }}
                        .summary {{ background: white; padding: 15px; margin: 15px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #eee; }}
                        .label {{ font-weight: bold; color: #555; }}
                        .value {{ color: #333; }}
                        .net-amount {{ background: #28a745; color: white; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; border-radius: 5px; margin-top: 15px; }}
                        .footer {{ text-align: center; color: #777; padding: 20px; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Payroll Notification</h2>
                            <p>{payrollRun.Name}</p>
                        </div>
        
                        <div class='content'>
                            <p>Dear <strong>{employee.FirstName} {employee.LastName}</strong>,</p>
                            <p>Your payroll for <strong>{payrollRun.PeriodStart:MMMM yyyy}</strong> has been processed.</p>
            
                            <div class='summary'>
                                <h3 style='margin-top: 0; color: #667eea;'>Payroll Summary</h3>
                
                                <div class='row'>
                                    <span class='label'>Period:</span>
                                    <span class='value'>{payrollRun.PeriodStart:dd MMM yyyy} - {payrollRun.PeriodEnd:dd MMM yyyy}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Base Salary:</span>
                                    <span class='value'>{entry.BaseSalaryAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Overtime:</span>
                                    <span class='value'>+{entry.OvertimeAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Bonus:</span>
                                    <span class='value'>+{entry.BonusAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Allowances:</span>
                                    <span class='value'>+{entry.AllowanceAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Deductions:</span>
                                    <span class='value'>-{entry.DeductionAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='row'>
                                    <span class='label'>Tax (10%):</span>
                                    <span class='value'>-{entry.TaxAmount:N2} {entry.Currency?.Code}</span>
                                </div>
                
                                <div class='net-amount'>
                                    NET AMOUNT: {entry.NetAmount:N2} {entry.Currency?.Code}
                                </div>
                            </div>
            
                            <p><strong>Bank Account:</strong> {entry.BankAccountNumber ?? "N/A"}</p>
            
                            {(!string.IsNullOrEmpty(entry.Notes) ? $"<p><strong>Notes:</strong> {entry.Notes}</p>" : "")}
            
                            <p>If you have any questions regarding your payroll, please contact HR Department.</p>
                        </div>
        
                        <div class='footer'>
                            <p>This is an automated notification. Please do not reply to this email.</p>
                            <p>&copy; {DateTime.Now.Year} ERP System - HR Department</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}
