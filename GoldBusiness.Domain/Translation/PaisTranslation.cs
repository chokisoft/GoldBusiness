using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class PaisTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int PaisId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public Pais Pais { get; private set; } = null!;

        protected PaisTranslation() { }

        public PaisTranslation(int paisId, string language, string descripcion, string creadoPor)
        {
            PaisId = paisId;
            EstablecerIdioma(language);
            SetDescripcion(descripcion, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetDescripcion(string descripcion, string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("El nombre es obligatorio.");

            if (descripcion.Length > 100)
                throw new DomainException("El nombre no puede exceder 100 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Descripcion = descripcion.Trim();
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}