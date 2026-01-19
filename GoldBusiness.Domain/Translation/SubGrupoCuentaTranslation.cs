using GoldBusiness.Domain.Entities;
using System;

namespace GoldBusiness.Domain.Translation
{
    public class SubGrupoCuentaTranslation
    {
        public int Id { get; private set; }
        public int SubGrupoCuentaId { get; private set; }
        public string Language { get; private set; } = string.Empty; // "es","en","fr"
        public string Descripcion { get; private set; } = string.Empty;

        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public SubGrupoCuenta SubGrupoCuenta { get; private set; } = null!;

        protected SubGrupoCuentaTranslation() { }

        public SubGrupoCuentaTranslation(int subGrupoCuentaId, string language, string descripcion, string creadoPor)
        {
            SubGrupoCuentaId = subGrupoCuentaId;
            Language = NormalizeLang(language);
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
        }

        public void SetDescripcion(string descripcion, string? modificadoPor)
        {
            Descripcion = descripcion ?? Descripcion;
            ModificadoPor = modificadoPor ?? ModificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        private static string NormalizeLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang)) return "es";
            var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return parts[0].ToLowerInvariant();
        }
    }
}