namespace ERP_System_Project.Extensions
{
    public static class FileExtension
    {
        public static bool IsValidFile(this IFormFile file, List<string> validExtensions, int maximumSize)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File cannot be null or empty.");

            if (file.Length > maximumSize)
                throw new Exception($"File size exceeds the maximum limit of {maximumSize}MB.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if(validExtensions.Contains(extension))
                throw new Exception($"Invalid file extension: {extension}. Expected: {String.Join(", ", validExtensions)}.");

            return true;
        }
    }
}
