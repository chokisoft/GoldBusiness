using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class ProductoTranslation
    {
        public int Id { get; private set; }
        public int ProductoId { get; private set; }
        public string Language { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Caracteristicas { get; private set; } = string.Empty;

        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public Producto Producto { get; private set; } = null!;

        protected ProductoTranslation() { }

        public ProductoTranslation(int productoId, string language, string descripcion, string caracteristicas, string creadoPor)
        {
            ProductoId = productoId;
            Language = NormalizeLang(language);
            SetDescripcion(descripcion, creadoPor);
            SetCaracteristicas(caracteristicas, creadoPor);
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
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

        public void SetCaracteristicas(string caracteristicas, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(caracteristicas) && caracteristicas.Length > 1024)
                throw new DomainException("Las características no pueden exceder 1024 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Caracteristicas = caracteristicas?.Trim() ?? string.Empty;
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