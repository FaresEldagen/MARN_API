using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs
{
public class GoogleLoginDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}
}