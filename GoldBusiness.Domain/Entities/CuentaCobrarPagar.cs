using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class CuentaCobrarPagar : BaseEntity
    {
        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public int TransaccionId { get; private set; }
        public DateTime Fecha { get; private set; }
        public int? ProveedorId { get; private set; }
        public int? ClienteId { get; private set; }
        public string NoPrimario { get; private set; } = string.Empty;
        public string NoDocumento { get; private set; } = string.Empty;
        public decimal Importe { get; private set; }

        // Pago Efectivo
        public int? CuentaPagoEfectivoId { get; private set; }
        public string PagoEfectivoDepartamento { get; private set; } = string.Empty;
        public decimal? PagoEfectivoImporte { get; private set; }
        public decimal? PagoEfectivoParcialMlc { get; private set; }

        // Pago Electrónico
        public int? CuentaPagoElectronicoId { get; private set; }
        public string PagoElectronicoDepartamento { get; private set; } = string.Empty;
        public decimal? PagoElectronicoImporte { get; private set; }
        public decimal? PagoElectronicoParcialMlc { get; private set; }

        // Cobro Efectivo
        public int? CuentaCobroEfectivoId { get; private set; }
        public string CobroEfectivoDepartamento { get; private set; } = string.Empty;
        public decimal? CobroEfectivoImporte { get; private set; }
        public decimal? CobroEfectivoParcialMlc { get; private set; }

        // Cobro Electrónico
        public int? CuentaCobroElectronicoId { get; private set; }
        public string CobroElectronicoDepartamento { get; private set; } = string.Empty;
        public decimal? CobroElectronicoImporte { get; private set; }
        public decimal? CobroElectronicoParcialMlc { get; private set; }

        public bool Contabilizada { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Establecimiento Establecimiento { get; private set; } = null!;
        public Transaccion Transaccion { get; private set; } = null!;
        public Proveedor? Proveedor { get; private set; }
        
        public Cliente Cliente { get; private set; } = null!;
        public Cuenta? CuentaPagoEfectivo { get; private set; }
        public Cuenta? CuentaPagoElectronico { get; private set; }
        public Cuenta? CuentaCobroEfectivo { get; private set; }
        public Cuenta? CuentaCobroElectronico { get; private set; }

        // Constructor protegido para EF Core
        protected CuentaCobrarPagar() { }

        // Constructor con validaciones
        public CuentaCobrarPagar(
            int establecimientoId,
            int transaccionId,
            DateTime fecha,
            string noDocumento,
            decimal importe,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            TransaccionId = transaccionId;
            SetFecha(fecha);
            SetNoDocumento(noDocumento);
            SetImporte(importe);
            EstablecerCreador(creadoPor);
            Cancelado = false;
            Contabilizada = false;
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

        public void SetNoDocumento(string noDocumento)
        {
            if (string.IsNullOrWhiteSpace(noDocumento))
                throw new DomainException("El número de documento es obligatorio.");

            if (noDocumento.Length > 50)
                throw new DomainException("El número de documento no puede exceder 50 caracteres.");

            NoDocumento = noDocumento.Trim();
        }

        public void SetImporte(decimal importe)
        {
            if (importe == 0)
                throw new DomainException("El importe no puede ser cero.");

            Importe = importe;
        }

        public void SetProveedor(int? proveedorId)
        {
            if (proveedorId.HasValue && ClienteId.HasValue)
                throw new DomainException("No se puede asignar proveedor y cliente simultáneamente.");

            ProveedorId = proveedorId;
        }

        public void SetCliente(int? clienteId)
        {
            if (clienteId.HasValue && ProveedorId.HasValue)
                throw new DomainException("No se puede asignar cliente y proveedor simultáneamente.");

            ClienteId = clienteId;
        }

        public void SetNoPrimario(string noPrimario)
        {
            if (!string.IsNullOrWhiteSpace(noPrimario) && noPrimario.Length > 50)
                throw new DomainException("El número primario no puede exceder 50 caracteres.");

            NoPrimario = noPrimario?.Trim() ?? string.Empty;
        }

        public void SetPagoEfectivo(int? cuentaId, string departamento, decimal? importe, decimal? parcialMlc)
        {
            if (importe.HasValue && importe.Value < 0)
                throw new DomainException("El importe de pago efectivo no puede ser negativo.");

            CuentaPagoEfectivoId = cuentaId;
            PagoEfectivoDepartamento = departamento?.Trim() ?? string.Empty;
            PagoEfectivoImporte = importe;
            PagoEfectivoParcialMlc = parcialMlc;
        }

        public void SetPagoElectronico(int? cuentaId, string departamento, decimal? importe, decimal? parcialMlc)
        {
            if (importe.HasValue && importe.Value < 0)
                throw new DomainException("El importe de pago electrónico no puede ser negativo.");

            CuentaPagoElectronicoId = cuentaId;
            PagoElectronicoDepartamento = departamento?.Trim() ?? string.Empty;
            PagoElectronicoImporte = importe;
            PagoElectronicoParcialMlc = parcialMlc;
        }

        public void SetCobroEfectivo(int? cuentaId, string departamento, decimal? importe, decimal? parcialMlc)
        {
            if (importe.HasValue && importe.Value < 0)
                throw new DomainException("El importe de cobro efectivo no puede ser negativo.");

            CuentaCobroEfectivoId = cuentaId;
            CobroEfectivoDepartamento = departamento?.Trim() ?? string.Empty;
            CobroEfectivoImporte = importe;
            CobroEfectivoParcialMlc = parcialMlc;
        }

        public void SetCobroElectronico(int? cuentaId, string departamento, decimal? importe, decimal? parcialMlc)
        {
            if (importe.HasValue && importe.Value < 0)
                throw new DomainException("El importe de cobro electrónico no puede ser negativo.");

            CuentaCobroElectronicoId = cuentaId;
            CobroElectronicoDepartamento = departamento?.Trim() ?? string.Empty;
            CobroElectronicoImporte = importe;
            CobroElectronicoParcialMlc = parcialMlc;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Contabilizar(string modificadoPor)
        {
            if (Contabilizada)
                throw new DomainException("La cuenta ya está contabilizada.");

            if (Cancelado)
                throw new DomainException("No se puede contabilizar una cuenta cancelada.");

            Contabilizada = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void DesContabilizar(string modificadoPor)
        {
            if (!Contabilizada)
                throw new DomainException("La cuenta no está contabilizada.");

            Contabilizada = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La cuenta ya está cancelada.");

            if (Contabilizada)
                throw new DomainException("No se puede cancelar una cuenta contabilizada.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("La cuenta no está cancelada.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA Y CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        public decimal GetTotalPagos()
        {
            return (PagoEfectivoImporte ?? 0) + (PagoElectronicoImporte ?? 0);
        }

        public decimal GetTotalCobros()
        {
            return (CobroEfectivoImporte ?? 0) + (CobroElectronicoImporte ?? 0);
        }

        public decimal GetSaldo()
        {
            return Importe - GetTotalPagos() - GetTotalCobros();
        }

        public bool EstaPagada()
        {
            return Math.Abs(GetSaldo()) < 0.01m;
        }

        public bool EsCuentaPorPagar()
        {
            return Importe < 0;
        }

        public bool EsCuentaPorCobrar()
        {
            return Importe > 0;
        }
    }
}