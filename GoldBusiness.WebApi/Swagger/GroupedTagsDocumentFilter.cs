using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GoldBusiness.WebApi.Swagger;

/// <summary>
/// Filtro para organizar tags en grupos jerárquicos en Swagger UI
/// </summary>
public class GroupedTagsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Definir los tags de grupo (secciones) con descripciones
        var groupTags = new Dictionary<string, OpenApiTag>
        {
            ["10. 🗂️ Nomencladores"] = new OpenApiTag
            {
                Name = "10. 🗂️ Nomencladores",
                Description = "Catálogos y clasificadores del sistema"
            },
            ["70. ⚙️ Configuración"] = new OpenApiTag
            {
                Name = "70. ⚙️ Configuración",
                Description = "Configuración general del sistema"
            }
        };

        // Agregar los tags de grupo al documento
        swaggerDoc.Tags ??= new List<OpenApiTag>();

        foreach (var groupTag in groupTags.Values)
        {
            if (!swaggerDoc.Tags.Any(t => t.Name == groupTag.Name))
            {
                swaggerDoc.Tags.Add(groupTag);
            }
        }

        // Ordenar todos los tags alfabéticamente (respetando los prefijos numéricos)
        swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(t => t.Name).ToList();
    }
}