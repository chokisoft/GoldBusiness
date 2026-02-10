using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class EstadoCuenta : BaseEntity
    {
        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public int CuentaId { get; private set; }
        public DateTime Fecha { get; private set; }
        public decimal Debito { get; private set; }
        public decimal Credito { get; private set; }
        public decimal Saldo { get; private set; }
        public string Referencia { get; private set; } = string.Empty;

        // Propiedades de navegación
        public Establecimiento Establecimiento { get; private set; } = null!;
        public Cuenta Cuenta { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected EstadoCuenta() { }

        // Constructor con validaciones
        public EstadoCuenta(
            int establecimientoId,
            int cuentaId,
            DateTime fecha,
            decimal debito,
            decimal credito,
            decimal saldo,
            string referencia,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            CuentaId = cuentaId;
            SetFecha(fecha);
            SetDebito(debito);
            SetCredito(credito);
            SetSaldo(saldo);
            SetReferencia(referencia);
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

        public void SetDebito(decimal debito)
        {
            if (debito < 0)
                throw new DomainException("El débito no puede ser negativo.");

            Debito = debito;
        }

        public void SetCredito(decimal credito)
        {
            if (credito < 0)
                throw new DomainException("El crédito no puede ser negativo.");

            Credito = credito;
        }

        public void SetSaldo(decimal saldo)
        {
            Saldo = saldo;
        }

        public void SetReferencia(string referencia)
        {
            if (!string.IsNullOrWhiteSpace(referencia) && referencia.Length > 256)
                throw new DomainException("La referencia no puede exceder 256 caracteres.");

            Referencia = referencia?.Trim() ?? string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public decimal GetMovimiento()
        {
            return Debito - Credito;
        }

        public bool EsDebito()
        {
            return Debito > Credito;
        }

        public bool EsCredito()
        {
            return Credito > Debito;
        }
    }
}