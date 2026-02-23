using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.Enums
{
    public enum ServiceResultType
    {
        Success,
        InvalidInput,    // 400 Bad Request
        Unauthorized,    // 401 Unauthorized
        NotFound,        // 404 Not Found
        Conflict,        // 409 Conflict (e.g., email already exists)
        InternalError   , // 500 Server Error
         BadRequest
    }
}