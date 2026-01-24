using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class SubLinea
    {
        private readonly HashSet<SubLineaTranslation> _translations = new();
        private readonly HashSet<Producto> _productos = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int LineaId { get; private set; }
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public Linea Linea { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<SubLineaTranslation> Translations => _translations;
        public IReadOnlyCollection<Producto> Productos => _productos;

        // Constructor protegido para EF Core
        protected SubLinea() { }

        // Constructor con validaciones
        public SubLinea(string codigo, string descripcion, int lineaId, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            LineaId = lineaId;
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
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
            var lang = NormalizeLang(language);
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
            var lang = NormalizeLang(language);
            var fb = NormalizeLang(fallback);

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
            
            if (_productos.Any(p => !p.Cancelado))
                throw new DomainException("No se puede cancelar una sublínea con productos activos.");
            
            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("La sublínea no está cancelada.");
            
            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private void ActualizarAuditoria(string usuario)
        {
            ModificadoPor = usuario ?? throw new ArgumentNullException(nameof(usuario));
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