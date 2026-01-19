using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GoldBusiness.WebApi.Swagger
{
    /// <summary>
    /// Filtro para agregar el parámetro Accept-Language en todos los endpoints de Swagger.
    /// Permite probar los mensajes de validación en diferentes idiomas.
    /// </summary>
    public class AcceptLanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Agregar parámetro Accept-Language en el header
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = "Idioma para los mensajes de validación y respuestas (es=Español, en=English, fr=Français)",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("es"),
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("es"),
                        new OpenApiString("en"),
                        new OpenApiString("fr")
                    }
                }
            });
        }
    }
}