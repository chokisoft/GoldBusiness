using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class SaldoAnterior : BaseEntity
    {
        public int Id { get; private set; }
        public int LocalidadId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal PrecioCosto { get; private set; }
        public decimal Existencia { get; private set; }
        public decimal ImporteCosto { get; private set; }
        public DateTime Fecha { get; private set; }

        // Propiedades de navegación
        public Localidad Localidad { get; private set; } = null!;
        public Producto Producto { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected SaldoAnterior() { }

        // Constructor con validaciones
        public SaldoAnterior(
            int localidadId,
            int productoId,
            decimal precioCosto,
            decimal existencia,
            DateTime fecha,
            string creadoPor)
        {
            LocalidadId = localidadId;
            ProductoId = productoId;
            Fecha = fecha;

            SetPrecioCosto(precioCosto);
            SetExistencia(existencia);

            EstablecerCreador(creadoPor);

            CalcularImporteCosto();
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetPrecioCosto(decimal precioCosto)
        {
            if (precioCosto < 0)
                throw new DomainException("El precio de costo no puede ser negativo.");

            PrecioCosto = precioCosto;
            CalcularImporteCosto();
        }

        public void SetExistencia(decimal existencia)
        {
            // Permitir existencias negativas para representar sobregiros históricos
            Existencia = existencia;
            CalcularImporteCosto();
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            Fecha = fecha;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        public void Ajustar(decimal precioCosto, decimal existencia, string modificadoPor)
        {
            SetPrecioCosto(precioCosto);
            SetExistencia(existencia);
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        private void CalcularImporteCosto()
        {
            ImporteCosto = Existencia * PrecioCosto;
        }

        public decimal GetValorInventario()
        {
            return ImporteCosto;
        }

        public decimal GetExistencia()
        {
            return Existencia;
        }

        public bool TieneSaldo()
        {
            return Existencia != 0;
        }

        public bool EsNegativo()
        {
            return Existencia < 0;
        }
    }
}