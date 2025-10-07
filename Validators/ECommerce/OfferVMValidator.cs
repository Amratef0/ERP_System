using ERP_System_Project.ViewModels.ECommerce;
using FluentValidation;

namespace ERP_System_Project.Validators.ECommerce
{
    public class OfferVMValidator : AbstractValidator<OfferVM>
    {
        public OfferVMValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage("Offer Name Is Required")
                .MaximumLength(50).WithMessage("Length Must Be Less Than 50 Character");

            RuleFor(o => o.DiscountPercentage)
                .NotEmpty().WithMessage("Discount Is Required")
                .Must(n => n >= 1 && n <= 100).WithMessage("Discount Percentage Range Allowed From 1% To 100%");

            RuleFor(o => o.OfferDays)
                .NotEmpty().WithMessage("Number Of Days Is Required")
                .LessThan(365).WithMessage("Discount Days Must Be Less Than 1 Year");
        }
    }
}
