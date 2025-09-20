using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ValidationAttributes
{
    public class ImageFileAttribute : ValidationAttribute
    {
        private readonly string[] _validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly int _maxFileSizeInMB;

        public ImageFileAttribute(int maxFileSizeInMB = 1)
        {
            _maxFileSizeInMB = maxFileSizeInMB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile formFile)
            {
                var file = value as IFormFile;

                if (file == null || file.Length == 0)
                    return new ValidationResult("Image file is required.");

                if (file.Length > _maxFileSizeInMB * 1024 * 1024)
                    return new ValidationResult($"Image size cannot exceed {_maxFileSizeInMB} MB.");

                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_validExtensions.Contains(extension))
                    return new ValidationResult($"Invalid image format: {extension}. Allowed formats: {string.Join(", ", _validExtensions)}");

                return ValidationResult.Success;
            }

            return new ValidationResult("The Field Must be a Valid Form File.");
        }
    }
}
