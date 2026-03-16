using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Provincia : BaseEntity
    {
        private readonly HashSet<ProvinciaTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int PaisId { get; private set; }
        public bool Cancelado { get; private set; }

        // Navegación
        public Pais Pais { get; private set; } = null!;
        public IReadOnlyCollection<ProvinciaTranslation> Translations => _translations;

        protected Provincia() { }

        public Provincia(
            string codigo,
            string descripcion,
            int paisId,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetPaisId(paisId);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (codigo.Length > 20)
                throw new DomainException("El código no puede exceder 20 caracteres.");

            Codigo = codigo.ToUpperInvariant();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 150)
                throw new DomainException("La descripción no puede exceder 150 caracteres.");

            Descripcion = descripcion.Trim();
        }

        public void SetPaisId(int paisId)
        {
            if (paisId <= 0)
                throw new DomainException("El ID del país debe ser mayor que cero.");

            PaisId = paisId;
        }

        public void Cancelar(string modificadoPor)
        {
            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Activar(string modificadoPor)
        {
            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void Actualizar(string descripcion, string modificadoPor)
        {
            SetDescripcion(descripcion);
            ActualizarAuditoria(modificadoPor);
        }

        public string GetDescripcion(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null && !string.IsNullOrWhiteSpace(fallbackMatch.Descripcion))
                return fallbackMatch.Descripcion;

            return Descripcion;
        }
    }
}