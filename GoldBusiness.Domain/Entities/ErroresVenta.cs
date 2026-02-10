using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class ErroresVenta : BaseEntity
    {
        public int Id { get; private set; }
        public int OperacionesDetalleId { get; private set; }
        public int LocalidadId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Cantidad { get; private set; }
        public decimal Costo { get; private set; }
        public decimal ImporteCosto { get; private set; }
        public bool Servicio { get; private set; }

        // Propiedades de navegación
        public Producto Producto { get; private set; } = null!;
        public Localidad Localidad { get; private set; } = null!;
        public OperacionesDetalle OperacionesDetalle { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected ErroresVenta() { }

        // Constructor con validaciones
        public ErroresVenta(
            int operacionesDetalleId,
            int localidadId,
            int productoId,
            decimal cantidad,
            decimal costo,
            bool servicio,
            string creadoPor)
        {
            OperacionesDetalleId = operacionesDetalleId;
            LocalidadId = localidadId;
            ProductoId = productoId;
            Servicio = servicio;

            SetCantidad(cantidad);
            SetCosto(costo);

            EstablecerCreador(creadoPor);

            CalcularImporteCosto();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCantidad(decimal cantidad)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor que cero.");

            Cantidad = cantidad;
            CalcularImporteCosto();
        }

        public void SetCosto(decimal costo)
        {
            if (costo < 0)
                throw new DomainException("El costo no puede ser negativo.");

            Costo = costo;
            CalcularImporteCosto();
        }

        public void SetServicio(bool servicio)
        {
            Servicio = servicio;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        public void Update(decimal cantidad, decimal costo, bool servicio, string modificadoPor)
        {
            SetCantidad(cantidad);
            SetCosto(costo);
            SetServicio(servicio);
            ActualizarAuditoria(modificadoPor);
        }

        public void Corregir(decimal cantidad, decimal costo, string modificadoPor)
        {
            SetCantidad(cantidad);
            SetCosto(costo);
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        private void CalcularImporteCosto()
        {
            try
            {
                ImporteCosto = checked(Cantidad * Costo);
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
    }
}