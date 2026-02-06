using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ComprobanteDetalleTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int ComprobanteDetalleId { get; private set; }
        public string Nota { get; private set; } = string.Empty;

        public ComprobanteDetalle ComprobanteDetalle { get; private set; } = null!;

        protected ComprobanteDetalleTranslation() { }

        public ComprobanteDetalleTranslation(int comprobanteDetalleId, string language, string nota, string creadoPor)
        {
            ComprobanteDetalleId = comprobanteDetalleId;
            EstablecerIdioma(language);
            SetNota(nota, creadoPor);
            EstablecerCreador(creadoPor);
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
    }
}