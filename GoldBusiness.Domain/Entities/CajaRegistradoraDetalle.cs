using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class CajaRegistradoraDetalle : BaseEntity
    {
        public int Id { get; private set; }
        public int CajaRegistradoraId { get; private set; }
        public int LocalidadId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Cantidad { get; private set; }
        public decimal Venta { get; private set; }
        public decimal ImporteVenta { get; private set; }

        // Propiedades de navegación
        public CajaRegistradora CajaRegistradora { get; private set; } = null!;
        public Localidad Localidad { get; private set; } = null!;
        public Producto Producto { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected CajaRegistradoraDetalle() { }

        // Constructor con validaciones
        public CajaRegistradoraDetalle(
            int cajaRegistradoraId,
            int localidadId,
            int productoId,
            decimal cantidad,
            decimal venta,
            string creadoPor)
        {
            CajaRegistradoraId = cajaRegistradoraId;
            LocalidadId = localidadId;
            ProductoId = productoId;
            SetCantidad(cantidad);
            SetVenta(venta);
            EstablecerCreador(creadoPor);

            CalcularImporteVenta();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCantidad(decimal cantidad)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor que cero.");

            Cantidad = cantidad;
            CalcularImporteVenta();
        }

        public void SetVenta(decimal venta)
        {
            if (venta < 0)
                throw new DomainException("El precio de venta no puede ser negativo.");

            Venta = venta;
            CalcularImporteVenta();
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA Y CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        public decimal GetImporteVenta()
        {
            return ImporteVenta;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private void CalcularImporteVenta()
        {
            ImporteVenta = Cantidad * Venta;
        }
    }
}