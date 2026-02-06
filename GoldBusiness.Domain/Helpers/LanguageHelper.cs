namespace GoldBusiness.Domain.Helpers
{
    /// <summary>
    /// Helper para manejo de códigos de idioma.
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// Normaliza el código de idioma a formato ISO 639-1 (2 letras).
        /// Ejemplos: "es-ES" -> "es", "en-US" -> "en"
        /// </summary>
        /// <param name="lang">Código de idioma (puede ser null, vacío o con formato regional).</param>
        /// <returns>Código de idioma normalizado en minúsculas (por defecto "es").</returns>
        public static string NormalizeLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                return "es";

            var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return parts[0].ToLowerInvariant();
        }
    }
}