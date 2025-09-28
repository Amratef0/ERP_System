using ERP_System_Project.Models;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Validators.HR
{
    public class PublicHolidayCountriesVMValidator : AbstractValidator<PublicHolidayCountriesVM>
    {
        private readonly Erpdbcontext _context;

        public PublicHolidayCountriesVMValidator(Erpdbcontext context)
        {
            _context = context;

            RuleFor(holiday => holiday.Name)
                .NotEmpty().WithMessage("Holiday name is required.")
                .MinimumLength(2).WithMessage("Holiday name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Holiday name cannot exceed 100 characters.")
                .MustAsync(async (vm, name, cancellationToken) =>
                {
                    if (vm.CountryId <= 0) return true;

                    return !await _context.PublicHolidays
                        .AnyAsync(ph => ph.Name == name && ph.Date == vm.Date && ph.CountryId == vm.CountryId && ph.Id != vm.Id, cancellationToken);
                })
                .WithMessage(vm => $"The holiday '{vm.Name}' already exists on {vm.Date.ToShortDateString()} in the selected country.");

            RuleFor(holiday => holiday.Date)
                .NotEmpty().WithMessage("Please select a date for the holiday.");

            RuleFor(holiday => holiday.CountryId)
                .GreaterThan(0).WithMessage("Please select a country.");
        }
    }
}
