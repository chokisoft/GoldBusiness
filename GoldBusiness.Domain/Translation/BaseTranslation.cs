using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Translation
{
    /// <summary>
    /// Clase base para entidades de traducción multiidioma.
    /// </summary>
    public abstract class BaseTranslation : BaseEntity
    {
        public string Language { get; protected set; } = string.Empty;

        /// <summary>
        /// Establece el idioma normalizado.
        /// </summary>
        protected void EstablecerIdioma(string? language)
        {
            Language = LanguageHelper.NormalizeLang(language);
        }
    }
}