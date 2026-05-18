namespace MARN_API.DTOs.Common
{
    public class ApiResponseDto<T>
    {
        public string Code { get; set; } = string.Empty;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
