using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ComprobanteTemporalTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int ComprobanteTemporalId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public ComprobanteTemporal ComprobanteTemporal { get; private set; } = null!;

        protected ComprobanteTemporalTranslation() { }

        public ComprobanteTemporalTranslation(int comprobanteTemporalId, string language, string descripcion, string creadoPor)
        {
            ComprobanteTemporalId = comprobanteTemporalId;
            EstablecerIdioma(language);
            SetDescripcion(descripcion, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetDescripcion(string descripcion, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Length > 512)
                throw new DomainException("La descripción no puede exceder 512 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Descripcion = descripcion?.Trim() ?? string.Empty;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}