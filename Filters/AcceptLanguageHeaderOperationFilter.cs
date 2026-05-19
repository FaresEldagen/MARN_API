using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MARN_API.Filters
{
    public class AcceptLanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (operation.Parameters.Any(parameter =>
                    string.Equals(parameter.Name, "Accept-Language", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Choose the response language. Supported values: en, ar.",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("en"),
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("en"),
                        new OpenApiString("ar")
                    }
                }
            });
        }
    }
}
