using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class CajaRegistradora : BaseEntity
    {
        private readonly HashSet<CajaRegistradoraDetalle> _detalles = new();

        public int Id { get; private set; }
        public int IdTurnoId { get; private set; }
        public int? Mesa { get; private set; }
        public bool Cerrado { get; private set; }

        // Propiedades de navegación
        public IdTurno IdTurnoNavigation { get; private set; } = null!;
        
        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<CajaRegistradoraDetalle> Detalles => _detalles;

        // Constructor protegido para EF Core
        protected CajaRegistradora() { }

        // Constructor con validaciones
        public CajaRegistradora(int idTurnoId, int? mesa, string creadoPor)
        {
            IdTurnoId = idTurnoId;
            SetMesa(mesa);
            EstablecerCreador(creadoPor);
            Cerrado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetMesa(int? mesa)
        {
            if (mesa.HasValue && mesa.Value < 0)
                throw new DomainException("El número de mesa no puede ser negativo.");

            Mesa = mesa;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Cerrar(string modificadoPor)
        {
            if (Cerrado)
                throw new DomainException("La caja ya está cerrada.");

            if (!_detalles.Any())
                throw new DomainException("No se puede cerrar una caja sin detalles.");

            Cerrado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Abrir(string modificadoPor)
        {
            if (!Cerrado)
                throw new DomainException("La caja no está cerrada.");

            Cerrado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA Y CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        public decimal GetTotalVenta()
        {
            return _detalles.Sum(d => d.ImporteVenta);
        }

        public int GetCantidadArticulos()
        {
            return _detalles.Count;
        }
    }
}