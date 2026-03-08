using System.Collections.Generic;

namespace GoldBusiness.CodeGen
{
    /// <summary>
    /// Metadatos completos de una entidad para generación de código
    /// </summary>
    public class EntityMetadata
    {
        public string EntityName { get; set; } = string.Empty;
        public string CamelCase { get; set; } = string.Empty;
        public string KebabCase { get; set; } = string.Empty;
        public string PluralEntityName { get; set; } = string.Empty;
        public string PluralCamelCase { get; set; } = string.Empty;
        public string TranslationKey { get; set; } = string.Empty;
        
        public List<PropertyMetadata> Properties { get; set; } = new();
        public List<ForeignKeyMetadata> ForeignKeys { get; set; } = new();
    }

    public class PropertyMetadata
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public int MaxLength { get; set; }
        public bool IsRequired { get; set; }
    }

    public class ForeignKeyMetadata
    {
        public string PropertyName { get; set; } = string.Empty;
        public string RelatedEntity { get; set; } = string.Empty;
    }
}