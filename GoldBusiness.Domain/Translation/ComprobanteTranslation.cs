using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ComprobanteTranslation
    {
        public int Id { get; private set; }
        public int ComprobanteId { get; private set; }
        public string Language { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;

        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public Comprobante Comprobante { get; private set; } = null!;

        protected ComprobanteTranslation() { }

        public ComprobanteTranslation(int comprobanteId, string language, string observaciones, string creadoPor)
        {
            ComprobanteId = comprobanteId;
            Language = NormalizeLang(language);
            SetObservaciones(observaciones, creadoPor);
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
        }

        public void SetObservaciones(string observaciones, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(observaciones) && observaciones.Length > 1024)
                throw new DomainException("Las observaciones no pueden exceder 1024 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Observaciones = observaciones?.Trim() ?? string.Empty;
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