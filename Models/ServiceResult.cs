using MARN_API.Enums;

namespace MARN_API.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public ServiceResultType ResultType { get; set; } // The "Why"

        public static ServiceResult<T> Ok(T data, string? message = null, ServiceResultType resultType = ServiceResultType.Success)
            => new() { Success = true, Data = data, Message = message, ResultType = resultType };

        public static ServiceResult<T> Fail(string message, List<string>? errors = null, ServiceResultType resultType = ServiceResultType.BadRequest)
            => new() { Success = false, Message = message, Errors = errors, ResultType = resultType };
    }
}
