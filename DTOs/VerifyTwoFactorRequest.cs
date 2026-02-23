using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs
{
public class VerifyTwoFactorDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}
}