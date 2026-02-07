using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Establecimiento : BaseEntity
    {
        private readonly HashSet<EstablecimientoTranslation> _translations = new();
        private readonly HashSet<Comprobante> _comprobantes = new();
        private readonly HashSet<ComprobanteTemporal> _comprobantesTemporales = new();
        private readonly HashSet<CuentaCobrarPagar> _cuentasCobrarPagar = new();
        private readonly HashSet<EstadoCuenta> _estadosCuenta = new();
        private readonly HashSet<Localidad> _localidades = new();
        private readonly HashSet<OperacionesEncabezado> _operacionesEncabezado = new();
        private readonly HashSet<Producto> _productos = new();

        public int Id { get; private set; }
        public int NegocioId { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Activo { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public SystemConfiguration NegocioNavigation { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<EstablecimientoTranslation> Translations => _translations;
        public IReadOnlyCollection<Comprobante> Comprobantes => _comprobantes;
        public IReadOnlyCollection<ComprobanteTemporal> ComprobantesTemporales => _comprobantesTemporales;
        public IReadOnlyCollection<CuentaCobrarPagar> CuentasCobrarPagar => _cuentasCobrarPagar;
        public IReadOnlyCollection<EstadoCuenta> EstadosCuenta => _estadosCuenta;
        public IReadOnlyCollection<Localidad> Localidades => _localidades;
        public IReadOnlyCollection<OperacionesEncabezado> OperacionesEncabezado => _operacionesEncabezado;
        public IReadOnlyCollection<Producto> Productos => _productos;

        // Constructor protegido para EF Core
        protected Establecimiento() { }

        // Constructor con validaciones
        public Establecimiento(string codigo, string descripcion, int negocioId, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            NegocioId = negocioId;
            EstablecerCreador(creadoPor);
            Activo = true;
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 6)
                throw new DomainException("El código debe tener exactamente 6 caracteres.");

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
                _translations.Add(new EstablecimientoTranslation(Id, lang, descripcion, usuario));
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

        public void Activar(string modificadoPor)
        {
            if (Activo)
                throw new DomainException("El establecimiento ya está activo.");

            Activo = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Desactivar(string modificadoPor)
        {
            if (!Activo)
                throw new DomainException("El establecimiento ya está desactivado.");

            Activo = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El establecimiento ya está cancelado.");

            if (_productos.Any(p => !p.Cancelado))
                throw new DomainException("No se puede cancelar un establecimiento con productos activos.");

            if (_localidades.Any(l => !l.Cancelado))
                throw new DomainException("No se puede cancelar un establecimiento con localidades activas.");

            Cancelado = true;
            Activo = false;
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

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public string GetCodigoDescripcion()
        {
            return $"{Codigo} | {Descripcion}";
        }

        public bool TieneProductosActivos()
        {
            return _productos.Any(p => !p.Cancelado);
        }

        public bool TieneLocalidadesActivas()
        {
            return _localidades.Any(l => !l.Cancelado);
        }

        public int GetCantidadProductos()
        {
            return _productos.Count(p => !p.Cancelado);
        }

        public int GetCantidadLocalidades()
        {
            return _localidades.Count(l => !l.Cancelado);
        }
    }
}