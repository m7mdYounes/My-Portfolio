using MyPortfolio.Helpers.Models;

namespace MyPortfolio.Helpers.Interfaces
{
    public interface IFileStorageHelper
    {
        Task<FileUploadResult> SaveImageAsync(
            IFormFile file,
            string folder,
            long maxFileSizeInBytes = 2 * 1024 * 1024,
            CancellationToken cancellationToken = default);

        Task<FileUploadResult> SavePdfAsync(
            IFormFile file,
            string folder,
            long maxFileSizeInBytes = 5 * 1024 * 1024,
            CancellationToken cancellationToken = default);

        Task<FileUploadResult> SaveFileAsync(
            IFormFile file,
            string folder,
            string[] allowedExtensions,
            string[] allowedContentTypes,
            long maxFileSizeInBytes,
            CancellationToken cancellationToken = default);

        bool DeleteFile(string? relativePath);

        string GetAbsolutePath(string relativePath);

        bool FileExists(string? relativePath);
    }
}
