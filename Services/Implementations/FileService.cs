using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var folderPath = Path.Combine(_env.WebRootPath, "images", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/{folderName}/{fileName}";
        }

        public void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
