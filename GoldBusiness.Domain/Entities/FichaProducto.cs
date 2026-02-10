using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class FichaProducto : BaseEntity
    {
        public int Id { get; private set; }
        public int ProductoId { get; private set; }
        public int LocalidadId { get; private set; }
        public int ComponenteId { get; private set; }
        public decimal Cantidad { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Producto Producto { get; private set; } = null!;
        public Producto Componente { get; private set; } = null!;
        public Localidad Localidad { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected FichaProducto() { }

        // Constructor con validaciones
        public FichaProducto(
            int productoId,
            int localidadId,
            int componenteId,
            decimal cantidad,
            string creadoPor)
        {
            ProductoId = productoId;
            LocalidadId = localidadId;
            ComponenteId = componenteId;
            
            SetCantidad(cantidad);
            ValidarComponente(productoId, componenteId);

            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCantidad(decimal cantidad)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor que cero.");

            Cantidad = cantidad;
        }

        private void ValidarComponente(int productoId, int componenteId)
        {
            if (productoId == componenteId)
                throw new DomainException("Un producto no puede ser componente de sí mismo.");
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(decimal cantidad, int localidadId, string modificadoPor)
        {
            SetCantidad(cantidad);
            LocalidadId = localidadId;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La ficha de producto ya está cancelada.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("La ficha de producto no está cancelada.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CÁLCULO Y UTILIDAD
        // ═══════════════════════════════════════════════════════════════

        public decimal CalcularCostoComponente(decimal costoUnitario)
        {
            return Cantidad * costoUnitario;
        }
    }
}