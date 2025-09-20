using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ValidationAttributes
{
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        public int MinimumAge { get; set; }
        public DateOfBirthValidationAttribute(int minAge= 18)
        {
            MinimumAge = minAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;
                if (age < 18)
                {
                    return new ValidationResult($"You must be at least {MinimumAge} years old.");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid date format.");
        }
    }
}
