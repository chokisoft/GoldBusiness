using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class OperacionesServicio : BaseEntity
    {
        public int Id { get; private set; }
        public int OperacionesDetalleId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Cantidad { get; private set; }
        public decimal Costo { get; private set; }
        public decimal ImporteCosto { get; private set; }
        public decimal Venta { get; private set; }
        public decimal ImporteVenta { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Producto Producto { get; private set; } = null!;
        public OperacionesDetalle OperacionesDetalle { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected OperacionesServicio() { }

        // Constructor con validaciones
        public OperacionesServicio(
            int operacionesDetalleId,
            int productoId,
            decimal cantidad,
            decimal costo,
            decimal venta,
            string creadoPor)
        {
            OperacionesDetalleId = operacionesDetalleId;
            ProductoId = productoId;

            SetCantidad(cantidad);
            SetCosto(costo);
            SetVenta(venta);

            EstablecerCreador(creadoPor);
            Cancelado = false;

            CalcularImportes();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCantidad(decimal cantidad)
        {
            if (cantidad < 0)
                throw new DomainException("La cantidad no puede ser negativa.");

            Cantidad = cantidad;
            CalcularImportes();
        }

        public void SetCosto(decimal costo)
        {
            if (costo < 0)
                throw new DomainException("El costo no puede ser negativo.");

            Costo = costo;
            CalcularImportes();
        }

        public void SetVenta(decimal venta)
        {
            if (venta < 0)
                throw new DomainException("El precio de venta no puede ser negativo.");

            Venta = venta;
            CalcularImportes();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        public void Update(int productoId, decimal cantidad, decimal costo, decimal venta, string modificadoPor)
        {
            ProductoId = productoId;
            SetCantidad(cantidad);
            SetCosto(costo);
            SetVenta(venta);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El servicio ya está cancelado.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El servicio no está cancelado.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        private void CalcularImportes()
        {
            try
            {
                ImporteCosto = checked(Cantidad * Costo);
                ImporteVenta = checked(Cantidad * Venta);
            }
            catch (OverflowException)
            {
                throw new DomainException("El cálculo excede el límite permitido.");
            }
        }

        public decimal GetImporteCosto()
        {
            return ImporteCosto;
        }

        public decimal GetImporteVenta()
        {
            return ImporteVenta;
        }

        public decimal GetMargenBruto()
        {
            return ImporteVenta - ImporteCosto;
        }

        public decimal GetPorcentajeMargen()
        {
            if (ImporteCosto == 0 || Costo == 0) return 0;
            return ((ImporteVenta - ImporteCosto) / ImporteCosto) * 100;
        }
    }
}