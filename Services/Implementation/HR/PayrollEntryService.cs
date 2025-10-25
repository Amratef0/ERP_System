using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class PayrollEntryService : GenericService<PayrollEntry>, IPayrollEntryService
    {
        private readonly IPayrollCalculationService _calculationService;
        private readonly IMapper _mapper;

        public PayrollEntryService(
            IUnitOfWork uow,
            IPayrollCalculationService calculationService,
            IMapper mapper) : base(uow)
        {
            _calculationService = calculationService;
            _mapper = mapper;
        }

        public async Task<(bool Success, PayrollEntry? Entry, string? ErrorMessage)> CreatePayrollEntryAsync(
            int payrollRunId,
            int employeeId,
            int year,
            int month)
        {
            try
            {
                // Get employee details
                var employee = await _uow.Employees
                    .GetAllAsIQueryable()
                    .Include(e => e.SalaryCurrency)
                    .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                    return (false, null, "Employee not found.");

                if (!employee.IsActive)
                    return (false, null, "Employee is not active.");

                // Get payroll run
                var payrollRun = await _uow.PayrollRuns.GetByIdAsync(payrollRunId);
                if (payrollRun == null)
                    return (false, null, "Payroll run not found.");

                if (payrollRun.IsLocked)
                    return (false, null, "Cannot add entries to a locked payroll run.");

                // Calculate components
                var baseSalary = employee.BaseSalary;
                var overtime = await _calculationService.CalculateOvertimeAmountAsync(employeeId, year, month);
                var bonus = await _calculationService.CalculateBonusAmountAsync(employeeId, year, month);
                var allowance = await _calculationService.CalculateAllowanceAmountAsync(employeeId, year, month);
                var deduction = await _calculationService.CalculateUnpaidLeaveDeductionAsync(employeeId, year, month);

                // Calculate tax (10% of gross)
                var grossSalary = baseSalary + overtime + bonus + allowance;
                var tax = _calculationService.CalculateTaxAmount(grossSalary);

                // Create entry
                var entry = new PayrollEntry
                {
                    PayrollRunId = payrollRunId,
                    EmployeeId = employeeId,
                    CurrencyId = employee.SalaryCurrencyId ?? 1, // Default to currency ID 1 if employee has no salary currency
                    BaseSalaryAmount = baseSalary,
                    OvertimeAmount = overtime,
                    BonusAmount = bonus,
                    AllowanceAmount = allowance,
                    DeductionAmount = deduction,
                    TaxAmount = tax,
                    BankAccountNumber = employee.BankAccountNumber,
                    // NetAmount is computed automatically by database
                };

                await _repository.AddAsync(entry);
                await _uow.CompleteAsync();

                return (true, entry, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error creating payroll entry: {ex.Message}");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdatePayrollEntryAsync(PayrollEntryVM model)
        {
            try
            {
                var entry = await _repository
                    .GetAllAsIQueryable()
                    .Include(pe => pe.PayrollRun)
                    .FirstOrDefaultAsync(pe => pe.Id == model.Id);

                if (entry == null)
                    return (false, "Payroll entry not found.");

                if (entry.PayrollRun.IsLocked)
                    return (false, "Cannot update entry in a locked payroll run.");

                // Update fields
                entry.BaseSalaryAmount = model.BaseSalaryAmount;
                entry.OvertimeAmount = model.OvertimeAmount;
                entry.BonusAmount = model.BonusAmount;
                entry.AllowanceAmount = model.AllowanceAmount;
                entry.DeductionAmount = model.DeductionAmount;
                entry.TaxAmount = model.TaxAmount;
                entry.PaymentDate = model.PaymentDate;
                entry.BankAccountNumber = model.BankAccountNumber;
                entry.Notes = model.Notes;
                // NetAmount recalculates automatically

                _repository.Update(entry);
                await _uow.CompleteAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error updating payroll entry: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PayrollEntryVM>> GetEntriesByPayrollRunAsync(int payrollRunId)
        {
            var entries = await _repository
                .GetAllAsIQueryable()
                .Include(pe => pe.Employee)
                    .ThenInclude(e => e.Department)
                .Include(pe => pe.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(pe => pe.Currency)
                .Include(pe => pe.PayrollRun)
                .Where(pe => pe.PayrollRunId == payrollRunId)
                .OrderBy(pe => pe.Employee.FirstName)
                .ThenBy(pe => pe.Employee.LastName)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PayrollEntryVM>>(entries);
        }

        public async Task<PayrollEntryVM?> GetPayrollEntryDetailsAsync(int entryId)
        {
            var entry = await _repository
                .GetAllAsIQueryable()
                .Include(pe => pe.Employee)
                    .ThenInclude(e => e.Department)
                .Include(pe => pe.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(pe => pe.Currency)
                .Include(pe => pe.PayrollRun)
                .FirstOrDefaultAsync(pe => pe.Id == entryId);

            return entry == null ? null : _mapper.Map<PayrollEntryVM>(entry);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeletePayrollEntryAsync(int entryId)
        {
            try
            {
                var entry = await _repository
                    .GetAllAsIQueryable()
                    .Include(pe => pe.PayrollRun)
                    .FirstOrDefaultAsync(pe => pe.Id == entryId);

                if (entry == null)
                    return (false, "Payroll entry not found.");

                if (entry.PayrollRun.IsLocked)
                    return (false, "Cannot delete entry from a locked payroll run.");

                _repository.Delete(entryId);
                await _uow.CompleteAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting payroll entry: {ex.Message}");
            }
        }

        public decimal CalculateNetAmount(decimal baseSalary, decimal overtime, decimal bonus,
            decimal allowance, decimal deduction, decimal tax)
        {
            return baseSalary + overtime + bonus + allowance - deduction - tax;
        }
    }
}
