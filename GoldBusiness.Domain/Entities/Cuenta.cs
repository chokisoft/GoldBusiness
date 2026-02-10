using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Cuenta : BaseEntity
    {
        private readonly HashSet<CuentaTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int SystemConfigurationId { get; set; }
        public int SubGrupoCuentaId { get; private set; }
        public SystemConfiguration SystemConfiguration { get; private set; } = null!;
        public SubGrupoCuenta SubGrupoCuenta { get; private set; } = null!;
        public bool Cancelado { get; private set; }

        public IReadOnlyCollection<CuentaTranslation> Translations => _translations;
        public IReadOnlyCollection<Localidad> LocalidadCuentaInventario { get; } = new HashSet<Localidad>();
        public IReadOnlyCollection<Localidad> LocalidadCuentaCosto { get; } = new HashSet<Localidad>();
        public IReadOnlyCollection<Localidad> LocalidadCuentaVenta { get; } = new HashSet<Localidad>();
        public IReadOnlyCollection<Localidad> LocalidadCuentaDevolucion { get; } = new HashSet<Localidad>();
        public IReadOnlyCollection<SystemConfiguration> ConfiguracionCuentaPagar { get; } = new HashSet<SystemConfiguration>();
        public IReadOnlyCollection<SystemConfiguration> ConfiguracionCuentaCobrar { get; } = new HashSet<SystemConfiguration>();

        // Constructor protegido para EF Core
        protected Cuenta() { }

        // Constructor con validaciones
        public Cuenta(string codigo, string descripcion, int systemConfigurationId, int subGrupoCuentaId, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            SystemConfigurationId = systemConfigurationId;
            SubGrupoCuentaId = subGrupoCuentaId;
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 8)
                throw new DomainException("El código debe tener exactamente 8 caracteres.");

            if (!codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser numérico (00000000-99999999).");

            Codigo = codigo;
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 256) // ✅ Agregada validación de longitud
                throw new DomainException("La descripción no puede exceder 256 caracteres.");

            Descripcion = descripcion.Trim(); // ✅ Agregado Trim()
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
                _translations.Add(new CuentaTranslation(Id, lang, descripcion, usuario));
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

        public void Update(string descripcion, int systemConfigurationId, int subGrupoCuentaId, string modificadoPor)
        {
            SetDescripcion(descripcion);
            SystemConfigurationId = systemConfigurationId;
            SubGrupoCuentaId = subGrupoCuentaId;
            ActualizarAuditoria(modificadoPor); // ✅ Usa método privado
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado) // ✅ Agregada validación
                throw new DomainException("La cuenta ya está cancelada.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor); // ✅ Usa método privado
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