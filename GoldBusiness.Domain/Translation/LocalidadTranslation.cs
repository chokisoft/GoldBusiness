using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class LocalidadTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int LocalidadId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public Localidad Localidad { get; private set; } = null!;

        protected LocalidadTranslation() { }

        public LocalidadTranslation(int localidadId, string language, string descripcion, string creadoPor)
        {
            LocalidadId = localidadId;
            EstablecerIdioma(language);
            SetDescripcion(descripcion, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetDescripcion(string descripcion, string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 256)
                throw new DomainException("La descripción no puede exceder 256 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Descripcion = descripcion.Trim();
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}