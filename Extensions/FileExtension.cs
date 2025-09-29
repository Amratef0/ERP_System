namespace ERP_System_Project.Extensions
{
    public static class FileExtension
    {

        public static async Task<string> SaveImageAsync(this IFormFile file, IWebHostEnvironment env, string folder = "uploads")
        {
            if (file == null || file.Length == 0)
                return null;

            // Build folder path inside wwwroot
            string folderPath = Path.Combine(env.WebRootPath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path (for database or HTML)
            return "/" + folder + "/" + uniqueFileName;
        }

        public static bool IsValidFile(this IFormFile file, List<string> validExtensions, int maximumSizeInMB = 1)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File Cannot Be Empty");

            if (file.Length > maximumSizeInMB * 1024 * 1024)
                throw new Exception($"File size exceeds the maximum limit of {maximumSizeInMB}MB");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if(validExtensions.Contains(extension))
                throw new Exception($"Invalid file extension: {extension}. Expected: {String.Join(", ", validExtensions)}");

            return true;
        }
    }
}
