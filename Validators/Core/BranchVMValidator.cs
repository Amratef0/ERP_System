using ERP_System_Project.ViewModels.Core;
using FluentValidation;

namespace ERP_System_Project.Validators.Core
{
    public class BranchVMValidator : AbstractValidator<BranchVM>
    {
        public BranchVMValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Branch Name Is Required")
                .MaximumLength(100).WithMessage("Branch Name Must be less than 100 characters");

            RuleFor(b => b.Code)
                .NotEmpty().WithMessage("Branch Code Is Required")
                .MaximumLength(10).WithMessage("Branch Code Must be less than 10 characters");

            RuleFor(b => b.PhoneNumber)
                .MaximumLength(50).WithMessage("Phone Number Must be less than 50 characters")
                .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid Phone Format, please enter numbers only.")
                .When(b => !string.IsNullOrEmpty(b.PhoneNumber));

            RuleFor(b => b.Email)
                .MaximumLength(50).WithMessage("Email Must be less than 50 characters")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid Email Format")
                .When(b => !string.IsNullOrEmpty(b.Email));

            RuleFor(b => b.Line1)
                .NotEmpty().WithMessage("Address Is Required")
                .MaximumLength(255).WithMessage("Address must be less than 255 characters");

            RuleFor(b => b.City)
                .NotEmpty().WithMessage("City Name Is Required")
                .MaximumLength(30).WithMessage("City must be less than 30 characters");

            RuleFor(b => b.StateProvince)
                .MaximumLength(30).WithMessage("State Province must be less than 30 characters");

            RuleFor(b => b.PostalCode)
                .MaximumLength(20).WithMessage("Postal Code must be less than 20 characters");

            RuleFor(b => b.AddressType)
                .MaximumLength(20).WithMessage("Address Type Name must be less than 20 characters");

            RuleFor(b => b.CountryId)
                .NotEmpty().WithMessage("Country is required.")
                .GreaterThan(0).WithMessage("Please select a valid country.");
        }
    }
}
