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
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is decimal decimalValue)
            {
                var scale = (decimal.GetBits(decimalValue)[3] >> 16) & 0x000000FF;
                if (scale > _scale)
                {
                    return new ValidationResult($"{validationContext.DisplayName} cannot have more than {_scale} digits after the decimal point.");
                }

                string valueAsString = decimalValue.ToString(CultureInfo.InvariantCulture);
                string[] parts = valueAsString.Split('.');
                int integerDigits = parts[0].StartsWith("-") ? parts[0].Length - 1 : parts[0].Length;
                int decimalDigits = parts.Length > 1 ? parts[1].Length : 0;

                if (integerDigits + decimalDigits > _precision)
                {
                    return new ValidationResult($"{validationContext.DisplayName} cannot exceed {_precision} digits in total.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} must be a valid decimal number.");
        }

    }
}