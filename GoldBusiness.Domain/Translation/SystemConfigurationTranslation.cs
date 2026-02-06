using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class SystemConfigurationTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int ConfiguracionId { get; private set; }
        public string NombreNegocio { get; private set; } = string.Empty;
        public string Direccion { get; private set; } = string.Empty;

        public Entities.SystemConfiguration Configuracion { get; private set; } = null!;

        protected SystemConfigurationTranslation() { }

        public SystemConfigurationTranslation(
            int configuracionId, 
            string language, 
            string nombreNegocio, 
            string direccion, 
            string creadoPor)
        {
            ConfiguracionId = configuracionId;
            EstablecerIdioma(language);
            SetNombreNegocio(nombreNegocio, creadoPor);
            SetDireccion(direccion, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetNombreNegocio(string nombreNegocio, string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(nombreNegocio))
                throw new DomainException("El nombre del negocio es obligatorio.");

            if (nombreNegocio.Length > 256)
                throw new DomainException("El nombre del negocio no puede exceder 256 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            NombreNegocio = nombreNegocio.Trim();
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        public void SetDireccion(string direccion, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(direccion) && direccion.Length > 512)
                throw new DomainException("La dirección no puede exceder 512 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Direccion = direccion?.Trim() ?? string.Empty;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}