namespace ERP_System_Project.Helpers
{
    public static class FileHelper
    {
        public static async Task DeleteImageFileAsync(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var filePath = Path.Combine("wwwroot/", imageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                }
            }
        }
    }
}
