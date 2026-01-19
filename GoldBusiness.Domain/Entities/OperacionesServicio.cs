using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class OperacionesServicio
    {
        public int Id { get; private set; }
        public int OperacionesDetalleId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Cantidad { get; private set; }
        public decimal Costo { get; private set; }
        public decimal ImporteCosto { get; private set; }
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        // Propiedades de navegación
        public Producto ProductoNavigation { get; private set; } = null!;
        public OperacionesDetalle OperacionesDetalleNavigation { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected OperacionesServicio() { }

        // Constructor con validaciones
        public OperacionesServicio(
            int operacionesDetalleId,
            int productoId,
            decimal cantidad,
            decimal costo,
            string creadoPor)
        {
            OperacionesDetalleId = operacionesDetalleId;
            ProductoId = productoId;

            SetCantidad(cantidad);
            SetCosto(costo);

            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Cancelado = false;

            CalcularImporteCosto();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCantidad(decimal cantidad)
        {
            if (cantidad < 0)
                throw new DomainException("La cantidad no puede ser negativa.");

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

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        public void Update(int productoId, decimal cantidad, decimal costo, string modificadoPor)
        {
            ProductoId = productoId;
            SetCantidad(cantidad);
            SetCosto(costo);
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

        private void CalcularImporteCosto()
        {
            ImporteCosto = Cantidad * Costo;
        }

        public decimal GetImporteCosto()
        {
            return ImporteCosto;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private void ActualizarAuditoria(string usuario)
        {
            ModificadoPor = usuario ?? throw new ArgumentNullException(nameof(usuario));
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}