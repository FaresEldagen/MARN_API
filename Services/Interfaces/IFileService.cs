namespace MARN_API.Services.Interfaces
{
    public interface IFileService
    {
        Task<string?> SaveImageAsync(IFormFile file, string folderName);
        void DeleteImage(string? imageUrl);
    }
}
