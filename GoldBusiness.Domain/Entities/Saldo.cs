using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class Saldo
    {
        public int Id { get; private set; }
        public int LocalidadId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Existencia { get; private set; }
        public DateTime Fecha { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        // Propiedades de navegación
        public Localidad LocalidadNavigation { get; private set; } = null!;
        public Producto ProductoNavigation { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected Saldo() { }

        // Constructor con validaciones
        public Saldo(
            int localidadId,
            int productoId,
            decimal existencia,
            DateTime fecha,
            string creadoPor)
        {
            LocalidadId = localidadId;
            ProductoId = productoId;
            Fecha = fecha;

            SetExistencia(existencia);

            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetExistencia(decimal existencia)
        {
            // Permitir existencias negativas para representar sobregiros
            Existencia = existencia;
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            Fecha = fecha;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y MOVIMIENTOS
        // ═══════════════════════════════════════════════════════════════

        public void AjustarExistencia(decimal nuevaExistencia, DateTime fecha, string modificadoPor)
        {
            SetExistencia(nuevaExistencia);
            SetFecha(fecha);
            ActualizarAuditoria(modificadoPor);
        }

        public void IncrementarExistencia(decimal cantidad, DateTime fecha, string modificadoPor)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad a incrementar debe ser mayor que cero.");

            Existencia += cantidad;
            SetFecha(fecha);
            ActualizarAuditoria(modificadoPor);
        }

        public void DecrementarExistencia(decimal cantidad, DateTime fecha, string modificadoPor)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad a decrementar debe ser mayor que cero.");

            Existencia -= cantidad;
            SetFecha(fecha);
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public bool TieneStock()
        {
            return Existencia > 0;
        }

        public bool EstaEnNegativo()
        {
            return Existencia < 0;
        }

        public bool EstaBajoDe(decimal umbral)
        {
            return Existencia < umbral;
        }

        public decimal GetExistencia()
        {
            return Existencia;
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