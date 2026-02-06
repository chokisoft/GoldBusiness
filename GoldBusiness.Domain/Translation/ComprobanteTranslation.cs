using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ComprobanteTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int ComprobanteId { get; private set; }
        public string Observaciones { get; private set; } = string.Empty;

        public Comprobante Comprobante { get; private set; } = null!;

        protected ComprobanteTranslation() { }

        public ComprobanteTranslation(int comprobanteId, string language, string observaciones, string creadoPor)
        {
            ComprobanteId = comprobanteId;
            EstablecerIdioma(language);
            SetObservaciones(observaciones, creadoPor);
            EstablecerCreador(creadoPor);
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
    }
}