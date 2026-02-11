using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class SubLinea : BaseEntity
    {
        private readonly HashSet<SubLineaTranslation> _translations = new();
        private readonly HashSet<Producto> _producto = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int LineaId { get; private set; }
        public bool Cancelado { get; private set; }

        public Linea Linea { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<SubLineaTranslation> Translations => _translations;
        public IReadOnlyCollection<Producto> Producto => _producto;

        // Constructor protegido para EF Core
        protected SubLinea() { }

        // Constructor con validaciones
        public SubLinea(string codigo, string descripcion, int lineaId, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            LineaId = lineaId;
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 5)
                throw new DomainException("El código debe tener exactamente 5 caracteres.");

            if (!codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser numérico (00000-99999).");

            Codigo = codigo;
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
                _translations.Add(new SubLineaTranslation(Id, lang, descripcion, usuario));
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

        public void Update(string descripcion, int lineaId, string modificadoPor)
        {
            SetDescripcion(descripcion);
            LineaId = lineaId;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La sublínea ya está cancelada.");
            
            if (_producto.Any(p => !p.Cancelado))
                throw new DomainException("No se puede cancelar una sublínea con productos activos.");
            
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