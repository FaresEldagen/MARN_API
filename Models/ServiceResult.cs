using MARN_API.Enums;

namespace MARN_API.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
        public object?[]? MessageArguments { get; set; }
        public T? Data { get; set; }
        public string? Action { get; set; }
        public List<string>? Errors { get; set; }
        public ServiceResultType ResultType { get; set; } // The "Why"

        public static ServiceResult<T> Ok(
            T data,
            string? message = null,
            ServiceResultType resultType = ServiceResultType.Success,
            string? code = null,
            object?[]? messageArguments = null)
            => new()
            {
                Success = true,
                Code = code,
                Data = data,
                Message = message,
                MessageArguments = messageArguments,
                ResultType = resultType
            };

        public static ServiceResult<T> Fail(
            string message,
            List<string>? errors = null,
            string? action = null,
            ServiceResultType resultType = ServiceResultType.BadRequest,
            string? code = null,
            object?[]? messageArguments = null)
            => new()
            {
                Success = false,
                Action = action,
                Code = code,
                Message = message,
                MessageArguments = messageArguments,
                Errors = errors,
                ResultType = resultType
            };
    }
}
