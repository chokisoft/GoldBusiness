using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class IdTurno : BaseEntity
    {
        private readonly HashSet<CajaRegistradora> _cajasRegistradoras = new();

        public int Id { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Cajero { get; private set; } = string.Empty;
        public DateTime Inicio { get; private set; }
        public decimal? Fondo { get; private set; }
        public decimal? Extraccion { get; private set; }
        public DateTime? Cierre { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<CajaRegistradora> CajasRegistradoras => _cajasRegistradoras;

        // Constructor protegido para EF Core
        protected IdTurno() { }

        // Constructor con validaciones
        public IdTurno(DateTime fecha, string cajero, DateTime inicio, decimal? fondo, string creadoPor)
        {
            SetFecha(fecha);
            SetCajero(cajero);
            SetInicio(inicio);
            SetFondo(fondo);
            EstablecerCreador(creadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            Fecha = fecha;
        }

        public void SetCajero(string cajero)
        {
            if (string.IsNullOrWhiteSpace(cajero))
                throw new DomainException("El cajero es obligatorio.");

            if (cajero.Length > 256)
                throw new DomainException("El cajero no puede exceder 256 caracteres.");

            Cajero = cajero.Trim();
        }

        public void SetInicio(DateTime inicio)
        {
            if (inicio == default)
                throw new DomainException("La hora de inicio es obligatoria.");

            Inicio = inicio;
        }

        public void SetFondo(decimal? fondo)
        {
            if (fondo.HasValue && fondo.Value < 0)
                throw new DomainException("El fondo no puede ser negativo.");

            Fondo = fondo;
        }

        public void SetExtraccion(decimal? extraccion)
        {
            if (extraccion.HasValue && extraccion.Value < 0)
                throw new DomainException("La extracción no puede ser negativa.");

            Extraccion = extraccion;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void CerrarTurno(DateTime cierre, decimal? extraccion, string modificadoPor)
        {
            if (Cierre.HasValue)
                throw new DomainException("El turno ya está cerrado.");

            if (cierre < Inicio)
                throw new DomainException("La hora de cierre no puede ser anterior a la de inicio.");

            Cierre = cierre;
            SetExtraccion(extraccion);
            ActualizarAuditoria(modificadoPor);
        }

        public void AbrirTurno(string modificadoPor)
        {
            if (!Cierre.HasValue)
                throw new DomainException("El turno no está cerrado.");

            Cierre = null;
            Extraccion = null;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public bool EstaCerrado()
        {
            return Cierre.HasValue;
        }

        public TimeSpan GetDuracion()
        {
            var fin = Cierre ?? DateTime.UtcNow;
            return fin - Inicio;
        }
    }
}