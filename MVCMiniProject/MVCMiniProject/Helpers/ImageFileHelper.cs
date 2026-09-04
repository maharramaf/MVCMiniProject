namespace MVCMiniProject.Helpers
{
    public static class ImageFileHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxBytes = 5 * 1024 * 1024;

        public static bool TryValidate(IFormFile? file, bool required, out string? error)
        {
            error = null;

            if (file == null || file.Length == 0)
            {
                if (required)
                {
                    error = "An image is required.";
                    return false;
                }

                return true;
            }

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                error = "Only JPG, PNG, WEBP or GIF images are allowed.";
                return false;
            }

            if (file.Length > MaxBytes)
            {
                error = "Image size must be 5 MB or less.";
                return false;
            }

            return true;
        }

        public static async Task<string> SaveAsync(IFormFile file, string webRootPath)
        {
            var directory = Path.Combine(webRootPath, "images");
            Directory.CreateDirectory(directory);

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
            var path = Path.Combine(directory, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public static void DeleteIfExists(string webRootPath, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var path = Path.Combine(webRootPath, "images", fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
