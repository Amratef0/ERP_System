using ERP_System_Project.ViewModels.HR;
using FluentValidation;

namespace ERP_System_Project.Validators.HR
{
    public class WorkScheduleDayVMValidator : AbstractValidator<WorkScheduleDayVM>
    {
        public WorkScheduleDayVMValidator()
        {
            RuleFor(vm => vm.WorkScheduleId)
                .GreaterThan(0).WithMessage("A work schedule must be assigned.");

            RuleFor(vm => vm.Day)
                .IsInEnum().WithMessage("Please select a valid day of the week.");

            RuleFor(vm => vm.WorkStartTime)
                .NotEmpty().WithMessage("Work Start Time is required on a work day.")
                .When(vm => vm.IsWorkDay);

            RuleFor(vm => vm.WorkEndTime)
                .NotEmpty().WithMessage("Work End Time is required on a work day.")
                .GreaterThan(vm => vm.WorkStartTime)
                .WithMessage("Work End Time must be after the start time.")
                .When(vm => vm.IsWorkDay);

            RuleFor(vm => vm.WorkStartTime)
                .Empty().WithMessage("Work Start Time should not be set for a day off.")
                .When(vm => !vm.IsWorkDay);

            RuleFor(vm => vm.WorkEndTime)
                .Empty().WithMessage("Work End Time should not be set for a day off.")
                .When(vm => !vm.IsWorkDay);
        }
    }
}
