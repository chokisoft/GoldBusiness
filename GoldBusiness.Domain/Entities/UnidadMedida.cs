using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class UnidadMedida : BaseEntity
    {
        private readonly HashSet<UnidadMedidaTranslation> _translations = new();
        private readonly HashSet<Producto> _producto = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<UnidadMedidaTranslation> Translations => _translations;
        public IReadOnlyCollection<Producto> Producto => _producto;

        // Constructor protegido para EF Core
        protected UnidadMedida() { }

        // Constructor con validaciones
        public UnidadMedida(string codigo, string descripcion, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 3)
                throw new DomainException("El código debe tener exactamente 3 caracteres.");

            Codigo = codigo.ToUpperInvariant();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 256)
                throw new DomainException("La descripción no puede exceder 256 caracteres.");

            Descripcion = descripcion.Trim();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string descripcion, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetDescripcion(descripcion, usuario);
            }
            else
            {
                _translations.Add(new UnidadMedidaTranslation(Id, lang, descripcion, usuario));
            }
        }

        public string GetDescripcion(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            if (!string.IsNullOrWhiteSpace(Descripcion))
                return Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Descripcion;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(string descripcion, string modificadoPor)
        {
            SetDescripcion(descripcion);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La unidad de medida ya está cancelada.");

            if (_producto.Any(p => !p.Cancelado))
                throw new DomainException("No se puede cancelar una unidad de medida con productos activos.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string? descripcion, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(descripcion))
            {
                SetDescripcion(descripcion);
            }

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }
    }
}