using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ERP_System_Project.Models.ValidationAttributes
{
    public class DecimalPrecisionScaleAttribute : ValidationAttribute
    {
        private readonly int _precision;
        private readonly int _scale;

        public DecimalPrecisionScaleAttribute(int precision, int scale)
        {
            _precision = precision;
            _scale = scale;
            ErrorMessage = $"The value cannot have more than {_precision - _scale} digits before the decimal point, and no more than {_scale} digits after it.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Let the [Required] attribute handle nulls.
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is decimal decimalValue)
            {
                // 1. Validate the scale (number of decimal places).
                var scale = (decimal.GetBits(decimalValue)[3] >> 16) & 0x000000FF;
                if (scale > _scale)
                {
                    return new ValidationResult($"The number of decimal places cannot exceed {_scale}.");
                }

                // 2. Validate the precision (total number of digits).
                string valueAsString = decimalValue.ToString(CultureInfo.InvariantCulture);
                string[] parts = valueAsString.Split('.');
                int integerDigits = parts[0].StartsWith("-") ? parts[0].Length - 1 : parts[0].Length;
                int decimalDigits = parts.Length > 1 ? parts[1].Length : 0;

                if (integerDigits + decimalDigits > _precision)
                {
                    return new ValidationResult($"The total number of digits cannot exceed {_precision}.");
                }

                // If all checks pass, the value is valid.
                return ValidationResult.Success;
            }

            return new ValidationResult("The field must be a valid decimal number.");
        }
    }
}