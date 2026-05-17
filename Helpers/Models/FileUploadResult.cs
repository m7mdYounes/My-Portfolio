namespace MyPortfolio.Helpers.Models
{
    public class FileUploadResult
    {
        public bool Succeeded { get; set; }

        public string? RelativePath { get; set; }

        public string? FileName { get; set; }

        public string? ErrorMessage { get; set; }

        public static FileUploadResult Success(string relativePath, string fileName)
        {
            return new FileUploadResult
            {
                Succeeded = true,
                RelativePath = relativePath,
                FileName = fileName
            };
        }

        public static FileUploadResult Failure(string errorMessage)
        {
            return new FileUploadResult
            {
                Succeeded = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
