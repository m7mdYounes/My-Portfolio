using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Helpers.Models;

namespace MyPortfolio.Helpers.Implementations
{
    public class FileStorageHelper : IFileStorageHelper
    {
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedImageExtensions =
        [
            ".jpg",
        ".jpeg",
        ".png",
        ".webp"
        ];

        private static readonly string[] AllowedImageContentTypes =
        [
            "image/jpeg",
        "image/png",
        "image/webp"
        ];

        private static readonly string[] AllowedPdfExtensions =
        [
            ".pdf"
        ];

        private static readonly string[] AllowedPdfContentTypes =
        [
            "application/pdf"
        ];

        public FileStorageHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<FileUploadResult> SaveImageAsync(
            IFormFile file,
            string folder,
            long maxFileSizeInBytes = 2 * 1024 * 1024,
            CancellationToken cancellationToken = default)
        {
            return await SaveFileAsync(
                file,
                folder,
                AllowedImageExtensions,
                AllowedImageContentTypes,
                maxFileSizeInBytes,
                cancellationToken);
        }

        public async Task<FileUploadResult> SavePdfAsync(
            IFormFile file,
            string folder,
            long maxFileSizeInBytes = 5 * 1024 * 1024,
            CancellationToken cancellationToken = default)
        {
            return await SaveFileAsync(
                file,
                folder,
                AllowedPdfExtensions,
                AllowedPdfContentTypes,
                maxFileSizeInBytes,
                cancellationToken);
        }

        public async Task<FileUploadResult> SaveFileAsync(
            IFormFile file,
            string folder,
            string[] allowedExtensions,
            string[] allowedContentTypes,
            long maxFileSizeInBytes,
            CancellationToken cancellationToken = default)
        {
            if (file is null)
                return FileUploadResult.Failure("No file was provided.");

            if (file.Length <= 0)
                return FileUploadResult.Failure("File is empty.");

            if (file.Length > maxFileSizeInBytes)
                return FileUploadResult.Failure($"File size cannot exceed {maxFileSizeInBytes / 1024 / 1024} MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension))
                return FileUploadResult.Failure("File extension is missing.");

            if (!allowedExtensions.Contains(extension))
                return FileUploadResult.Failure($"File extension '{extension}' is not allowed.");

            if (!allowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                return FileUploadResult.Failure($"File content type '{file.ContentType}' is not allowed.");

            var safeFolder = NormalizeFolder(folder);
            var absoluteFolderPath = Path.Combine(_environment.WebRootPath, safeFolder);

            if (!Directory.Exists(absoluteFolderPath))
                Directory.CreateDirectory(absoluteFolderPath);

            var fileName = GenerateFileName(extension);
            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileName);

            await using var stream = new FileStream(absoluteFilePath, FileMode.CreateNew);

            await file.CopyToAsync(stream, cancellationToken);

            var relativePath = "/" + safeFolder.Replace("\\", "/") + "/" + fileName;

            return FileUploadResult.Success(relativePath, fileName);
        }

        public bool DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            var absolutePath = GetAbsolutePath(relativePath);

            if (!File.Exists(absolutePath))
                return false;

            File.Delete(absolutePath);
            return true;
        }

        public string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Relative path cannot be empty.", nameof(relativePath));

            var normalizedPath = relativePath
                .TrimStart('/')
                .Replace("/", Path.DirectorySeparatorChar.ToString());

            return Path.Combine(_environment.WebRootPath, normalizedPath);
        }

        public bool FileExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            var absolutePath = GetAbsolutePath(relativePath);

            return File.Exists(absolutePath);
        }

        private static string NormalizeFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Folder cannot be empty.", nameof(folder));

            return folder
                .Trim()
                .TrimStart('/')
                .TrimEnd('/')
                .Replace("/", Path.DirectorySeparatorChar.ToString());
        }

        private static string GenerateFileName(string extension)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var randomPart = Guid.NewGuid().ToString("N")[..12];

            return $"{timestamp}_{randomPart}{extension}";
        }
    }
}
