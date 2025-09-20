namespace ERP_System_Project.Extensions
{
    public static class FileExtension
    {
        public static bool IsValidFile(this IFormFile file, List<string> validExtensions, int maximumSize = 1)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File Cannot Be Empty");

            if (file.Length > maximumSize * 1024 * 1024)
                throw new Exception($"File size exceeds the maximum limit of {maximumSize}MB");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if(validExtensions.Contains(extension))
                throw new Exception($"Invalid file extension: {extension}. Expected: {String.Join(", ", validExtensions)}");

            return true;
        }
    }
}
