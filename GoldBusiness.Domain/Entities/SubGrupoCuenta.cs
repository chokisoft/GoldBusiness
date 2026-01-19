using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class SubGrupoCuenta
    {
        private readonly HashSet<Cuenta> _cuenta = new();
        private readonly HashSet<SubGrupoCuentaTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public int GrupoCuentaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public bool Deudora { get; private set; }
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        // Propiedades de navegación
        public GrupoCuenta GrupoCuenta { get; private set; } = null!;
        
        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<Cuenta> Cuenta => _cuenta;
        public IReadOnlyCollection<SubGrupoCuentaTranslation> Translations => _translations;

        // Constructor protegido para EF Core
        protected SubGrupoCuenta() { }

        // Constructor con validaciones
        public SubGrupoCuenta(
            string codigo,
            int grupoCuentaId,
            string descripcion,
            bool deudora,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            GrupoCuentaId = grupoCuentaId;
            Deudora = deudora;
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 5 || !codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser un número de 5 dígitos.");

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

        public void SetDeudora(bool deudora)
        {
            Deudora = deudora;
        }

        public void AgregarCuenta(Cuenta cuenta)
        {
            if (cuenta == null)
                throw new DomainException("La cuenta no puede ser nula.");

            _cuenta.Add(cuenta);
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
                _translations.Add(new SubGrupoCuentaTranslation(Id, lang, descripcion, usuario));
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

        public void Update(string descripcion, int grupoCuentaId, bool deudora, string modificadoPor)
        {
            SetDescripcion(descripcion);
            GrupoCuentaId = grupoCuentaId;
            SetDeudora(deudora);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El subgrupo de cuenta ya está cancelado.");

            if (_cuenta.Any(c => !c.Cancelado))
                throw new DomainException("No se puede cancelar un subgrupo de cuenta con cuentas activas.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El subgrupo de cuenta no está cancelado.");

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