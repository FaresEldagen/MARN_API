using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Common
{
    public class SeedCsvUploadDto
    {
        [Required]
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; } = null!;
    }
}
