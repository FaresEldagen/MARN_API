namespace MARN_API.DTOs.Common
{
    public class ApiResponseDto<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
