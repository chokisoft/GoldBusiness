using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Localidad : BaseEntity
    {
        private readonly HashSet<LocalidadTranslation> _translations = new();
        private readonly HashSet<ErroresVenta> _erroresVenta = new();
        private readonly HashSet<FichaProducto> _fichaProductos = new();
        private readonly HashSet<OperacionesDetalle> _operacionesDetalle = new();
        private readonly HashSet<Saldo> _saldos = new();
        private readonly HashSet<SaldoAnterior> _saldosAnteriores = new();

        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Almacen { get; private set; }
        public int CuentaInventarioId { get; private set; }
        public int CuentaCostoId { get; private set; }
        public int CuentaVentaId { get; private set; }
        public int CuentaDevolucionId { get; private set; }
        public bool Activo { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Establecimiento Establecimiento { get; private set; } = null!;
        public Cuenta CuentaInventario { get; private set; } = null!;
        public Cuenta CuentaCosto { get; private set; } = null!;
        public Cuenta CuentaVenta { get; private set; } = null!;
        public Cuenta CuentaDevolucion { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<LocalidadTranslation> Translations => _translations;
        public IReadOnlyCollection<ErroresVenta> ErroresVenta => _erroresVenta;
        public IReadOnlyCollection<FichaProducto> FichaProductos => _fichaProductos;
        public IReadOnlyCollection<OperacionesDetalle> OperacionesDetalle => _operacionesDetalle;
        public IReadOnlyCollection<Saldo> Saldos => _saldos;
        public IReadOnlyCollection<SaldoAnterior> SaldosAnteriores => _saldosAnteriores;

        // Constructor protegido para EF Core
        protected Localidad() { }

        // Constructor con validaciones
        public Localidad(
            int establecimientoId,
            string codigo,
            string descripcion,
            int cuentaInventarioId,
            int cuentaCostoId,
            int cuentaVentaId,
            int cuentaDevolucionId,
            bool almacen,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            CuentaInventarioId = cuentaInventarioId;
            CuentaCostoId = cuentaCostoId;
            CuentaVentaId = cuentaVentaId;
            CuentaDevolucionId = cuentaDevolucionId;
            Almacen = almacen;

            SetCodigo(codigo);
            SetDescripcion(descripcion);

            EstablecerCreador(creadoPor);
            Activo = true;
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 9)
                throw new DomainException("El código debe tener exactamente 9 caracteres.");

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

        public void SetCuentas(
            int cuentaInventarioId,
            int cuentaCostoId,
            int cuentaVentaId,
            int cuentaDevolucionId)
        {
            CuentaInventarioId = cuentaInventarioId;
            CuentaCostoId = cuentaCostoId;
            CuentaVentaId = cuentaVentaId;
            CuentaDevolucionId = cuentaDevolucionId;
        }

        public void SetAlmacen(bool almacen)
        {
            Almacen = almacen;
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
                _translations.Add(new LocalidadTranslation(Id, lang, descripcion, usuario));
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

        public void Update(
            string descripcion,
            int cuentaInventarioId,
            int cuentaCostoId,
            int cuentaVentaId,
            int cuentaDevolucionId,
            bool almacen,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            SetCuentas(cuentaInventarioId, cuentaCostoId, cuentaVentaId, cuentaDevolucionId);
            SetAlmacen(almacen);
            ActualizarAuditoria(modificadoPor);
        }

        public void Activar(string modificadoPor)
        {
            if (Activo)
                throw new DomainException("La localidad ya está activa.");

            Activo = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Desactivar(string modificadoPor)
        {
            if (!Activo)
                throw new DomainException("La localidad ya está desactivada.");

            Activo = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La localidad ya está cancelada.");

            if (_saldos.Any(s => s.TieneStock()))
                throw new DomainException("No se puede cancelar una localidad con inventario.");

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

        public bool TieneInventario()
        {
            return _saldos.Any(s => s.TieneStock());
        }

        public int GetCantidadProductosConStock()
        {
            return _saldos.Count(s => s.TieneStock());
        }

        public decimal GetValorTotalInventario()
        {
            // Esto requeriría join con productos para calcular valor
            // Por ahora retorna 0, se calculará en la capa de aplicación
            return 0;
        }
    }
}