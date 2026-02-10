using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    public class OperacionesDetalle : BaseEntity
    {
        private readonly HashSet<ErroresVenta> _erroresVenta = new();
        private readonly HashSet<OperacionesServicio> _operacionesServicio = new();

        public int Id { get; private set; }
        public int OperacionesEncabezadoId { get; private set; }
        public int LocalidadId { get; private set; }
        public int ProductoId { get; private set; }
        public decimal Cantidad { get; private set; }
        public decimal Costo { get; private set; }
        public decimal ImporteCosto { get; private set; }
        public decimal Venta { get; private set; }
        public decimal ImporteVenta { get; private set; }
        public decimal Existencia { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Producto Producto { get; private set; } = null!;
        public Localidad Localidad { get; private set; } = null!;
        public OperacionesEncabezado OperacionEncabezado { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ErroresVenta> ErroresVenta => _erroresVenta;
        public IReadOnlyCollection<OperacionesServicio> OperacionesServicio => _operacionesServicio;

        // Constructor protegido para EF Core
        protected OperacionesDetalle() { }

        // Constructor con validaciones
        public OperacionesDetalle(
            int operacionesEncabezadoId,
            int localidadId,
            int productoId,
            decimal cantidad,
            decimal costo,
            decimal venta,
            decimal existencia,
            string creadoPor)
        {
            OperacionesEncabezadoId = operacionesEncabezadoId;
            LocalidadId = localidadId;
            ProductoId = productoId;
            Existencia = existencia;

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
            if (cantidad == 0)
                throw new DomainException("La cantidad no puede ser cero.");

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

        public void SetExistencia(decimal existencia)
        {
            Existencia = existencia;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(
            decimal cantidad,
            decimal costo,
            decimal venta,
            decimal existencia,
            string modificadoPor)
        {
            SetCantidad(cantidad);
            SetCosto(costo);
            SetVenta(venta);
            SetExistencia(existencia);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El detalle ya está cancelado.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El detalle no está cancelado.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 GESTIÓN DE COLECCIONES - ERRORES DE VENTA
        // ═══════════════════════════════════════════════════════════════

        public void AgregarErrorVenta(ErroresVenta error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (Cancelado)
                throw new DomainException("No se pueden agregar errores a un detalle cancelado.");

            if (error.OperacionesDetalleId != 0 && error.OperacionesDetalleId != Id)
                throw new DomainException("El error de venta pertenece a otro detalle de operación.");

            if (_erroresVenta.Any(e => e.Id == error.Id && error.Id != 0))
                throw new DomainException("El error de venta ya existe en la colección.");

            _erroresVenta.Add(error);
        }

        public void RemoverErrorVenta(ErroresVenta error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (!_erroresVenta.Contains(error))
                throw new DomainException("El error de venta no existe en la colección.");

            _erroresVenta.Remove(error);
        }

        public void LimpiarErroresVenta()
        {
            if (Cancelado)
                throw new DomainException("No se pueden limpiar errores de un detalle cancelado.");

            _erroresVenta.Clear();
        }

        public bool TieneErroresVenta()
        {
            return _erroresVenta.Count > 0;
        }

        public ErroresVenta? BuscarErrorVentaPorId(int errorId)
        {
            return _erroresVenta.FirstOrDefault(e => e.Id == errorId);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 GESTIÓN DE COLECCIONES - SERVICIOS
        // ═══════════════════════════════════════════════════════════════

        public void AgregarServicio(OperacionesServicio servicio)
        {
            if (servicio == null)
                throw new ArgumentNullException(nameof(servicio));

            if (Cancelado)
                throw new DomainException("No se pueden agregar servicios a un detalle cancelado.");

            if (servicio.OperacionesDetalleId != 0 && servicio.OperacionesDetalleId != Id)
                throw new DomainException("El servicio pertenece a otro detalle de operación.");

            if (_operacionesServicio.Any(s => s.Id == servicio.Id && servicio.Id != 0))
                throw new DomainException("El servicio ya existe en la colección.");

            _operacionesServicio.Add(servicio);
        }

        public void RemoverServicio(OperacionesServicio servicio)
        {
            if (servicio == null)
                throw new ArgumentNullException(nameof(servicio));

            if (!_operacionesServicio.Contains(servicio))
                throw new DomainException("El servicio no existe en la colección.");

            _operacionesServicio.Remove(servicio);
        }

        public void LimpiarServicios()
        {
            if (Cancelado)
                throw new DomainException("No se pueden limpiar servicios de un detalle cancelado.");

            _operacionesServicio.Clear();
        }

        public bool TieneServicios()
        {
            return _operacionesServicio.Count > 0;
        }

        public OperacionesServicio? BuscarServicioPorId(int servicioId)
        {
            return _operacionesServicio.FirstOrDefault(s => s.Id == servicioId);
        }

        public decimal GetTotalServicios()
        {
            return _operacionesServicio.Sum(s => s.ImporteVenta);
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

        public decimal GetMargenBruto()
        {
            return ImporteVenta - ImporteCosto;
        }

        public decimal GetPorcentajeMargen()
        {
            if (ImporteCosto == 0 || Costo == 0) return 0;
            return ((ImporteVenta - ImporteCosto) / ImporteCosto) * 100;
        }

        public bool EsVenta()
        {
            return Cantidad > 0;
        }

        public bool EsDevolucion()
        {
            return Cantidad < 0;
        }
    }
}