namespace MyPortfolio.Services.Models
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }

        public string? Message { get; set; }

        public List<string> Errors { get; set; } = new();

        public static ServiceResult Success(string? message = null)
        {
            return new ServiceResult
            {
                Succeeded = true,
                Message = message
            };
        }

        public static ServiceResult Failure(string error)
        {
            return new ServiceResult
            {
                Succeeded = false,
                Errors = new List<string> { error }
            };
        }

        public static ServiceResult Failure(IEnumerable<string> errors)
        {
            return new ServiceResult
            {
                Succeeded = false,
                Errors = errors.ToList()
            };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data, string? message = null)
        {
            return new ServiceResult<T>
            {
                Succeeded = true,
                Data = data,
                Message = message
            };
        }

        public new static ServiceResult<T> Failure(string error)
        {
            return new ServiceResult<T>
            {
                Succeeded = false,
                Errors = new List<string> { error }
            };
        }

        public new static ServiceResult<T> Failure(IEnumerable<string> errors)
        {
            return new ServiceResult<T>
            {
                Succeeded = false,
                Errors = errors.ToList()
            };
        }
    }
}
