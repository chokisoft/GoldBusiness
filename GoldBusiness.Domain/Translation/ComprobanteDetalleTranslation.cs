using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ComprobanteDetalleTranslation
    {
        public int Id { get; private set; }
        public int ComprobanteDetalleId { get; private set; }
        public string Language { get; private set; } = string.Empty;
        public string Nota { get; private set; } = string.Empty;

        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public ComprobanteDetalle ComprobanteDetalle { get; private set; } = null!;

        protected ComprobanteDetalleTranslation() { }

        public ComprobanteDetalleTranslation(int comprobanteDetalleId, string language, string nota, string creadoPor)
        {
            ComprobanteDetalleId = comprobanteDetalleId;
            Language = NormalizeLang(language);
            SetNota(nota, creadoPor);
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
        }

        public void SetNota(string nota, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(nota) && nota.Length > 512)
                throw new DomainException("La nota no puede exceder 512 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Nota = nota?.Trim() ?? string.Empty;
            ModificadoPor = modificadoPor;
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