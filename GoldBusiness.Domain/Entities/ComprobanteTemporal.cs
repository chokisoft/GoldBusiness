using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class ComprobanteTemporal
    {
        private readonly HashSet<ComprobanteTemporalTranslation> _translations = new();

        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public string CodigoTransaccion { get; private set; } = string.Empty;
        public string Transaccion { get; private set; } = string.Empty;
        public string NoDocumento { get; private set; } = string.Empty;
        public DateTime Fecha { get; private set; }
        public string Cuenta { get; private set; } = string.Empty;
        public string Departamento { get; private set; } = string.Empty;
        public bool Inventario { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public decimal? Debito { get; private set; }
        public decimal? Credito { get; private set; }
        public decimal? Parcial { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }

        // Propiedades de navegación
        public Establecimiento EstablecimientoNavigation { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ComprobanteTemporalTranslation> Translations => _translations;

        // Constructor protegido para EF Core
        protected ComprobanteTemporal() { }

        // Constructor con validaciones
        public ComprobanteTemporal(
            int establecimientoId,
            string codigoTransaccion,
            string transaccion,
            string noDocumento,
            DateTime fecha,
            string cuenta,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            SetCodigoTransaccion(codigoTransaccion);
            SetTransaccion(transaccion);
            SetNoDocumento(noDocumento);
            SetFecha(fecha);
            SetCuenta(cuenta);
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Inventario = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigoTransaccion(string codigoTransaccion)
        {
            if (string.IsNullOrWhiteSpace(codigoTransaccion))
                throw new DomainException("El código de transacción es obligatorio.");

            if (codigoTransaccion.Length > 10)
                throw new DomainException("El código de transacción no puede exceder 10 caracteres.");

            CodigoTransaccion = codigoTransaccion.Trim().ToUpperInvariant();
        }

        public void SetTransaccion(string transaccion)
        {
            if (string.IsNullOrWhiteSpace(transaccion))
                throw new DomainException("La transacción es obligatoria.");

            if (transaccion.Length > 256)
                throw new DomainException("La transacción no puede exceder 256 caracteres.");

            Transaccion = transaccion.Trim();
        }

        public void SetNoDocumento(string noDocumento)
        {
            if (string.IsNullOrWhiteSpace(noDocumento))
                throw new DomainException("El número de documento es obligatorio.");

            if (noDocumento.Length > 50)
                throw new DomainException("El número de documento no puede exceder 50 caracteres.");

            NoDocumento = noDocumento.Trim();
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            Fecha = fecha;
        }

        public void SetCuenta(string cuenta)
        {
            if (string.IsNullOrWhiteSpace(cuenta))
                throw new DomainException("La cuenta es obligatoria.");

            if (cuenta.Length > 8)
                throw new DomainException("La cuenta no puede exceder 8 caracteres.");

            Cuenta = cuenta.Trim();
        }

        public void SetDepartamento(string departamento)
        {
            if (!string.IsNullOrWhiteSpace(departamento) && departamento.Length > 50)
                throw new DomainException("El departamento no puede exceder 50 caracteres.");

            Departamento = departamento?.Trim() ?? string.Empty;
        }

        public void SetDescripcion(string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Length > 512)
                throw new DomainException("La descripción no puede exceder 512 caracteres.");

            Descripcion = descripcion?.Trim() ?? string.Empty;
        }

        public void SetInventario(bool inventario)
        {
            Inventario = inventario;
        }

        public void SetDebito(decimal? debito)
        {
            if (debito.HasValue && debito.Value < 0)
                throw new DomainException("El débito no puede ser negativo.");

            if (debito.HasValue && debito.Value > 0 && Credito.HasValue && Credito.Value > 0)
                throw new DomainException("No se puede tener débito y crédito simultáneamente.");

            Debito = debito;
        }

        public void SetCredito(decimal? credito)
        {
            if (credito.HasValue && credito.Value < 0)
                throw new DomainException("El crédito no puede ser negativo.");

            if (credito.HasValue && credito.Value > 0 && Debito.HasValue && Debito.Value > 0)
                throw new DomainException("No se puede tener crédito y débito simultáneamente.");

            Credito = credito;
        }

        public void SetParcial(decimal? parcial)
        {
            Parcial = parcial;
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
                _translations.Add(new ComprobanteTemporalTranslation(Id, lang, descripcion, usuario));
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
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public decimal GetMonto()
        {
            return (Debito ?? 0) > 0 ? (Debito ?? 0) : (Credito ?? 0);
        }

        public bool EsDebito()
        {
            return Debito.HasValue && Debito.Value > 0;
        }

        public bool EsCredito()
        {
            return Credito.HasValue && Credito.Value > 0;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private static string NormalizeLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang)) return "es";
            var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return parts[0].ToLowerInvariant();
        }
    }
}