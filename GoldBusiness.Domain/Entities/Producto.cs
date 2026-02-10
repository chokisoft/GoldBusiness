using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Producto : BaseEntity
    {
        private readonly HashSet<ProductoTranslation> _translations = new();
        private readonly HashSet<ErroresVenta> _erroresVenta = new();
        private readonly HashSet<FichaProducto> _fichaProductoCodigo = new();
        private readonly HashSet<FichaProducto> _fichaProductoProducto = new();
        private readonly HashSet<OperacionesDetalle> _operacionesDetalle = new();
        private readonly HashSet<OperacionesServicio> _operacionesServicio = new();
        private readonly HashSet<Saldo> _saldos = new();
        private readonly HashSet<SaldoAnterior> _saldosAnteriores = new();

        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int UnidadMedidaId { get; private set; }
        public int ProveedorId { get; private set; }
        public decimal PrecioVenta { get; private set; }
        public decimal PrecioCosto { get; private set; }
        public decimal Iva { get; private set; }
        public string CodigoReferencia { get; private set; } = string.Empty;
        public decimal StockMinimo { get; private set; }
        public bool Servicio { get; private set; }
        public int SubLineaId { get; private set; }
        public byte[] Imagen { get; private set; } = Array.Empty<byte>();
        public string Caracteristicas { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Establecimiento Establecimiento { get; private set; } = null!;
        public Proveedor Proveedor { get; private set; } = null!;
        public SubLinea SubLinea { get; private set; } = null!;
        public UnidadMedida UnidadMedida { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ProductoTranslation> Translations => _translations;
        public IReadOnlyCollection<ErroresVenta> ErroresVenta => _erroresVenta;
        public IReadOnlyCollection<FichaProducto> FichaProductoCodigo => _fichaProductoCodigo;
        public IReadOnlyCollection<FichaProducto> FichaProductoProducto => _fichaProductoProducto;
        public IReadOnlyCollection<OperacionesDetalle> OperacionesDetalle => _operacionesDetalle;
        public IReadOnlyCollection<OperacionesServicio> OperacionesServicio => _operacionesServicio;
        public IReadOnlyCollection<Saldo> Saldos => _saldos;
        public IReadOnlyCollection<SaldoAnterior> SaldosAnteriores => _saldosAnteriores;

        // Constructor protegido para EF Core
        protected Producto() { }

        // Constructor con validaciones
        public Producto(
            int establecimientoId,
            string codigo,
            string descripcion,
            int unidadMedidaId,
            int proveedorId,
            int subLineaId,
            decimal precioVenta,
            decimal precioCosto,
            decimal iva,
            string creadoPor,
            bool servicio = false)
        {
            EstablecimientoId = establecimientoId;
            UnidadMedidaId = unidadMedidaId;
            ProveedorId = proveedorId;
            SubLineaId = subLineaId;
            Servicio = servicio;

            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetPrecios(precioVenta, precioCosto, iva);

            EstablecerCreador(creadoPor);
            Cancelado = false;
            StockMinimo = 0;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (codigo.Length > 13)
                throw new DomainException("El código no puede exceder 13 caracteres.");

            Codigo = codigo.Trim();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 256)
                throw new DomainException("La descripción no puede exceder 256 caracteres.");

            Descripcion = descripcion.Trim();
        }

        public void SetCaracteristicas(string caracteristicas)
        {
            if (!string.IsNullOrWhiteSpace(caracteristicas) && caracteristicas.Length > 1024)
                throw new DomainException("Las características no pueden exceder 1024 caracteres.");

            Caracteristicas = caracteristicas?.Trim() ?? string.Empty;
        }

        public void SetPrecios(decimal precioVenta, decimal precioCosto, decimal iva)
        {
            if (precioVenta < 0)
                throw new DomainException("El precio de venta no puede ser negativo.");

            if (precioCosto < 0)
                throw new DomainException("El precio de costo no puede ser negativo.");

            if (iva < -0.01m || iva > 99.99m)
                throw new DomainException("El IVA debe estar entre -0.01 y 99.99.");

            PrecioVenta = precioVenta;
            PrecioCosto = precioCosto;
            Iva = iva;
        }

        public void SetCodigoReferencia(string codigoReferencia)
        {
            if (!string.IsNullOrWhiteSpace(codigoReferencia) && codigoReferencia.Length > 50)
                throw new DomainException("El código de referencia no puede exceder 50 caracteres.");

            CodigoReferencia = codigoReferencia?.Trim() ?? string.Empty;
        }

        public void SetStockMinimo(decimal stockMinimo)
        {
            if (stockMinimo < 0)
                throw new DomainException("El stock mínimo no puede ser negativo.");

            StockMinimo = stockMinimo;
        }

        public void SetImagen(byte[] imagen)
        {
            Imagen = imagen ?? Array.Empty<byte>();
        }

        public void SetServicio(bool servicio)
        {
            Servicio = servicio;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string descripcion, string caracteristicas, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetDescripcion(descripcion, usuario);
                existing.SetCaracteristicas(caracteristicas, usuario);
            }
            else
            {
                _translations.Add(new ProductoTranslation(Id, lang, descripcion, caracteristicas, usuario));
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

        public string GetCaracteristicas(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Caracteristicas))
                return match.Caracteristicas;

            if (!string.IsNullOrWhiteSpace(Caracteristicas))
                return Caracteristicas;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Caracteristicas;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(
            string descripcion,
            int unidadMedidaId,
            int proveedorId,
            int subLineaId,
            decimal precioVenta,
            decimal precioCosto,
            decimal iva,
            string codigoReferencia,
            decimal stockMinimo,
            bool servicio,
            string caracteristicas,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            UnidadMedidaId = unidadMedidaId;
            ProveedorId = proveedorId;
            SubLineaId = subLineaId;
            SetPrecios(precioVenta, precioCosto, iva);
            SetCodigoReferencia(codigoReferencia);
            SetStockMinimo(stockMinimo);
            SetServicio(servicio);
            SetCaracteristicas(caracteristicas);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El producto ya está cancelado.");

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

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CÁLCULO Y UTILIDAD
        // ═══════════════════════════════════════════════════════════════

        public decimal GetPrecioVentaConIva()
        {
            return PrecioVenta * (1 + (Iva / 100));
        }

        public decimal GetMargenBeneficio()
        {
            if (PrecioCosto == 0) return 0;
            return ((PrecioVenta - PrecioCosto) / PrecioCosto) * 100;
        }

        public bool TieneImagen()
        {
            return Imagen != null && Imagen.Length > 0;
        }
    }
}