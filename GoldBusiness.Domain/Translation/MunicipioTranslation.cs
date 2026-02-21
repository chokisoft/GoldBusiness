using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class MunicipioTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int MunicipioId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public Municipio Municipio { get; private set; } = null!;

        protected MunicipioTranslation() { }

        public MunicipioTranslation(int municipioId, string language, string descripcion, string creadoPor)
        {
            MunicipioId = municipioId;
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