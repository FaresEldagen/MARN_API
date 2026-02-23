using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs
{

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage ="invalid Email address")]
    public string Email { get; set; } = string.Empty;
}
}