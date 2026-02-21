namespace MARN_API.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ServiceResult<T> Ok(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ServiceResult<T> Fail(string message, List<string>? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }
}
