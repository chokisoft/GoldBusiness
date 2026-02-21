using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ProvinciaTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int ProvinciaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public Provincia Provincia { get; private set; } = null!;

        protected ProvinciaTranslation() { }

        public ProvinciaTranslation(int provinciaId, string language, string descripcion, string creadoPor)
        {
            ProvinciaId = provinciaId;
            EstablecerIdioma(language);
            SetDescripcion(descripcion, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetDescripcion(string descripcion, string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 150)
                throw new DomainException("La descripción no puede exceder 150 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Descripcion = descripcion.Trim();
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}