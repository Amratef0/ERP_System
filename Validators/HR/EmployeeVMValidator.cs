using ERP_System_Project.ViewModels.HR;
using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;

namespace ERP_System_Project.Validators.HR
{
    public class EmployeeVMValidator : AbstractValidator<EmployeeVM>
    {
        private readonly string[] _validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public EmployeeVMValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(e => e.Gender).IsInEnum();

            RuleFor(e => e.HireDate)
                .NotEmpty().WithMessage("Hire date is required.");

            RuleFor(e => e.TerminationDate)
                .GreaterThanOrEqualTo(e => e.HireDate)
                .When(e => e.TerminationDate.HasValue)
                .WithMessage("Termination date must be on or after the hire date.");

            RuleFor(e => e.WorkEmail)
                .EmailAddress().WithMessage("A valid email address is required.")
                .MinimumLength(5).WithMessage("Work email must be at least 5 characters long.")
                .MaximumLength(255).WithMessage("Work email cannot exceed 255 characters.")
                .When(e => !string.IsNullOrEmpty(e.WorkEmail));

            RuleFor(e => e.WorkPhone)
                .MinimumLength(5).WithMessage("Work phone must be at least 5 characters long.")
                .MaximumLength(50).WithMessage("Work phone cannot exceed 50 characters.")
                .When(e => !string.IsNullOrEmpty(e.WorkPhone));

            RuleFor(e => e.PersonalEmail)
                .EmailAddress().WithMessage("A valid personal email address is required.")
                .MinimumLength(5).WithMessage("Personal email must be at least 5 characters long.")
                .MaximumLength(255).WithMessage("Personal email cannot exceed 255 characters.")
                .When(e => !string.IsNullOrEmpty(e.PersonalEmail));

            RuleFor(e => e.PersonalPhone)
                .MinimumLength(5).WithMessage("Personal phone must be at least 5 characters long.")
                .MaximumLength(50).WithMessage("Personal phone must be at least 5 characters long.")
                .When(e => !string.IsNullOrEmpty(e.PersonalPhone));

            RuleFor(e => e.EmergencyContactName)
                .MinimumLength(2).WithMessage("Emergency contact name must be at least 2 characters long.")
                .MaximumLength(255).WithMessage("Emergency contact name cannot exceed 255 characters.")
                .When(e => !string.IsNullOrEmpty(e.EmergencyContactName));

            RuleFor(e => e.EmergencyContactPhone)
                .MinimumLength(5).WithMessage("Emergency contact phone cannot exceed 50 characters.")
                .MaximumLength(50).WithMessage("Emergency contact phone cannot exceed 50 characters.")
                .When(e => !string.IsNullOrEmpty(e.EmergencyContactPhone));

            RuleFor(e => e.BaseSalary)
                .NotEmpty().WithMessage("Base salary is required.")
                .GreaterThan(0).WithMessage("Base salary must be greater than zero.");

            RuleFor(e => e.BankAccountNumber)
                .MinimumLength(1).WithMessage("Bank account number must be at least 1 character long.")
                .MaximumLength(50).WithMessage("Bank account number cannot exceed 50 characters.")
                .When(e => !string.IsNullOrEmpty(e.BankAccountNumber));

            RuleFor(e => e.BankName)
                .MinimumLength(1).WithMessage("Bank name must be at least 1 character long.")
                .MaximumLength(50).WithMessage("Bank name cannot exceed 50 characters.")
                .When(e => !string.IsNullOrEmpty(e.BankName));

            RuleFor(x => x.Image)
                .Must(Image => _validExtensions.Contains(Path.GetExtension(Image.FileName))).WithMessage($"Invalid image extension. Allowed formats: {string.Join(", ", _validExtensions)}")
                .Must(Image => Image.Length <= 1 * 1024 * 1024).WithMessage("Image size cannot exceed 1 MB.")
                .When(x => x.Image != null);

            RuleFor(x => x.NewImage)
                .Must(NewImage => _validExtensions.Contains(Path.GetExtension(NewImage.FileName))).WithMessage($"Invalid image extension. Allowed formats: {string.Join(", ", _validExtensions)}")
                .Must(NewImage => NewImage.Length <= 1 * 1024 * 1024).WithMessage("Image size cannot exceed 1 MB.")
                .When(x => x.NewImage != null);

            RuleFor(e => e.Line1)
                .NotEmpty().WithMessage("Address Is Required")
                .MaximumLength(255).WithMessage("Address must be less than 255 characters");

            RuleFor(e => e.Line2)
                .MaximumLength(255).WithMessage("Second Address must be less than 255 characters")
                .When(e => !string.IsNullOrEmpty(e.Line2));

            RuleFor(e => e.City)
                .NotEmpty().WithMessage("City Name Is Required")
                .MaximumLength(30).WithMessage("City must be less than 30 characters");

            RuleFor(e => e.StateProvince)
                .MaximumLength(30).WithMessage("State Province must be less than 30 characters")
                .When(e => !string.IsNullOrEmpty(e.StateProvince));

            RuleFor(e => e.PostalCode)
                .MaximumLength(20).WithMessage("Postal Code must be less than 20 characters")
                .When(e => !string.IsNullOrEmpty(e.PostalCode));
        }
    }
}
