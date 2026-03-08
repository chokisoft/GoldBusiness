using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoldBusiness.CodeGen
{
    /// <summary>
    /// Analiza archivos de entidades C# y extrae metadatos completos
    /// </summary>
    public class EntityAnalyzer
    {
        public EntityMetadata AnalyzeEntity(string entityFilePath, string entityName)
        {
            if (!File.Exists(entityFilePath))
                throw new FileNotFoundException($"No se encontró el archivo: {entityFilePath}");

            var lines = File.ReadAllLines(entityFilePath);
            var metadata = new EntityMetadata
            {
                EntityName = entityName,
                CamelCase = ToCamelCase(entityName),
                KebabCase = ToKebabCase(entityName),
                PluralEntityName = ToPluralEntityName(entityName),
                PluralCamelCase = ToCamelCase(ToPluralEntityName(entityName)),
                TranslationKey = ToKebabCase(entityName)
            };

            Console.WriteLine($"📋 Analizando entidad: {entityName}");

            // Analizar propiedades
            metadata.Properties = ExtractProperties(lines);
            Console.WriteLine($"   ✓ {metadata.Properties.Count} propiedades encontradas");

            // Analizar FKs
            metadata.ForeignKeys = ExtractForeignKeys(lines, metadata.Properties);
            Console.WriteLine($"   ✓ {metadata.ForeignKeys.Count} relaciones encontradas");

            return metadata;
        }

        private List<PropertyMetadata> ExtractProperties(string[] lines)
        {
            var properties = new List<PropertyMetadata>();
            
            // Regex para propiedades públicas: public TipoNombre { get/set }
            var propertyRegex = new Regex(@"public\s+(?<Type>\w+\??)\s+(?<Name>\w+)\s*\{\s*(get|private\sset)");

            // Regex para atributos como [MaxLength(256)]
            var maxLengthRegex = new Regex(@"\[MaxLength\((\d+)\)\]");
            var requiredRegex = new Regex(@"\[Required\]");

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                var match = propertyRegex.Match(line);

                if (match.Success)
                {
                    var propType = match.Groups["Type"].Value;
                    var propName = match.Groups["Name"].Value;

                    // Ignorar colecciones de navegación
                    if (propType.StartsWith("IReadOnlyCollection") || 
                        propType.StartsWith("ICollection") ||
                        propType.StartsWith("HashSet") ||
                        propName.Contains("Translations"))
                        continue;

                    var isNullable = propType.EndsWith("?");
                    var baseType = propType.TrimEnd('?');

                    var property = new PropertyMetadata
                    {
                        Name = propName,
                        Type = baseType,
                        IsNullable = isNullable || baseType == "string"
                    };

                    // Buscar atributos en líneas anteriores
                    if (i > 0)
                    {
                        var previousLine = lines[i - 1].Trim();
                        
                        var maxLengthMatch = maxLengthRegex.Match(previousLine);
                        if (maxLengthMatch.Success)
                        {
                            property.MaxLength = int.Parse(maxLengthMatch.Groups[1].Value);
                        }

                        if (requiredRegex.IsMatch(previousLine))
                        {
                            property.IsRequired = true;
                            property.IsNullable = false;
                        }
                    }

                    properties.Add(property);
                }
            }

            return properties;
        }

        private List<ForeignKeyMetadata> ExtractForeignKeys(string[] lines, List<PropertyMetadata> properties)
        {
            var foreignKeys = new List<ForeignKeyMetadata>();

            // Buscar propiedades que terminen en "Id" y tengan una propiedad de navegación correspondiente
            var idProperties = properties.Where(p => p.Name.EndsWith("Id") && p.Name != "Id").ToList();

            foreach (var idProp in idProperties)
            {
                // El nombre de la entidad relacionada es el nombre de la propiedad sin "Id"
                var relatedEntityName = idProp.Name.Substring(0, idProp.Name.Length - 2);

                // Verificar si existe una propiedad de navegación con ese nombre
                var navigationRegex = new Regex($@"public\s+{relatedEntityName}\s+{relatedEntityName}\s*{{\s*get");
                var hasNavigation = lines.Any(line => navigationRegex.IsMatch(line));

                if (hasNavigation)
                {
                    foreignKeys.Add(new ForeignKeyMetadata
                    {
                        PropertyName = idProp.Name,
                        RelatedEntity = relatedEntityName
                    });
                }
            }

            return foreignKeys;
        }

        private string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToLower(str[0]) + str.Substring(1);
        }

        private string ToKebabCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }

        private string ToPluralEntityName(string entityName)
        {
            // Reglas simples de pluralización en español
            if (entityName.EndsWith("a")) return entityName + "s";
            if (entityName.EndsWith("o")) return entityName + "s";
            if (entityName.EndsWith("Cuenta")) return entityName + "s";
            return entityName + "s";
        }
    }
}