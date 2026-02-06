using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class OperacionesEncabezado : BaseEntity
    {
        private readonly HashSet<OperacionesEncabezadoTranslation> _translations = new();
        private readonly HashSet<OperacionesDetalle> _operacionesDetalle = new();

        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public int TransaccionId { get; private set; }
        public string NoDocumento { get; private set; } = string.Empty;
        public DateTime Fecha { get; private set; }
        public int? ProveedorId { get; private set; }
        public int? ClienteId { get; private set; }
        public string NoPrimario { get; private set; } = string.Empty;
        public int? ReferenciaId { get; private set; }
        public int? ConceptoAjusteId { get; private set; }
        public string Concepto { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;
        public bool Efectivo { get; private set; }
        public bool Contabilizada { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Establecimiento EstablecimientoNavigation { get; private set; } = null!;
        public Transaccion TransaccionNavigation { get; private set; } = null!;
        public Proveedor? ProveedorNavigation { get; private set; }
        public Cliente? ClienteNavigation { get; private set; }
        public ConceptoAjuste? ConceptoAjusteNavigation { get; private set; }

        public IReadOnlyCollection<OperacionesEncabezadoTranslation> Translations => _translations;
        public IReadOnlyCollection<OperacionesDetalle> OperacionesDetalle => _operacionesDetalle;

        // Constructor protegido para EF Core
        protected OperacionesEncabezado() { }

        // Constructor con validaciones
        public OperacionesEncabezado(
            int establecimientoId,
            int transaccionId,
            string noDocumento,
            DateTime fecha,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            TransaccionId = transaccionId;
            SetNoDocumento(noDocumento);
            SetFecha(fecha);
            EstablecerCreador(creadoPor);
            Cancelado = false;
            Efectivo = false;
            Contabilizada = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetNoDocumento(string noDocumento)
        {
            if (string.IsNullOrWhiteSpace(noDocumento))
                throw new DomainException("El número de documento es obligatorio.");

            if (noDocumento.Length > 50)
                throw new DomainException("El número de documento no puede exceder 50 caracteres.");

            NoDocumento = noDocumento.Trim();
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            if (fecha > DateTime.UtcNow.AddDays(1))
                throw new DomainException("La fecha no puede ser futura.");

            Fecha = fecha;
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

        public void SetReferencia(int? referenciaId)
        {
            ReferenciaId = referenciaId;
        }

        public void SetConceptoAjuste(int? conceptoAjusteId)
        {
            ConceptoAjusteId = conceptoAjusteId;
        }

        public void SetConcepto(string concepto)
        {
            if (!string.IsNullOrWhiteSpace(concepto) && concepto.Length > 512)
                throw new DomainException("El concepto no puede exceder 512 caracteres.");

            Concepto = concepto?.Trim() ?? string.Empty;
        }

        public void SetObservaciones(string observaciones)
        {
            if (!string.IsNullOrWhiteSpace(observaciones) && observaciones.Length > 1024)
                throw new DomainException("Las observaciones no pueden exceder 1024 caracteres.");

            Observaciones = observaciones?.Trim() ?? string.Empty;
        }

        public void SetEfectivo(bool efectivo)
        {
            Efectivo = efectivo;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string concepto, string observaciones, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetConcepto(concepto, usuario);
                existing.SetObservaciones(observaciones, usuario);
            }
            else
            {
                _translations.Add(new OperacionesEncabezadoTranslation(Id, lang, concepto, observaciones, usuario));
            }
        }

        public string GetConcepto(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Concepto))
                return match.Concepto;

            if (!string.IsNullOrWhiteSpace(Concepto))
                return Concepto;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Concepto;

            return string.Empty;
        }

        public string GetObservaciones(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Observaciones))
                return match.Observaciones;

            if (!string.IsNullOrWhiteSpace(Observaciones))
                return Observaciones;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Observaciones;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(
            DateTime fecha,
            int? proveedorId,
            int? clienteId,
            string noPrimario,
            int? referenciaId,
            int? conceptoAjusteId,
            string concepto,
            string observaciones,
            bool efectivo,
            string modificadoPor)
        {
            SetFecha(fecha);
            SetProveedor(proveedorId);
            SetCliente(clienteId);
            SetNoPrimario(noPrimario);
            SetReferencia(referenciaId);
            SetConceptoAjuste(conceptoAjusteId);
            SetConcepto(concepto);
            SetObservaciones(observaciones);
            SetEfectivo(efectivo);
            ActualizarAuditoria(modificadoPor);
        }

        public void Contabilizar(string modificadoPor)
        {
            if (Contabilizada)
                throw new DomainException("La operación ya está contabilizada.");

            if (Cancelado)
                throw new DomainException("No se puede contabilizar una operación cancelada.");

            if (!_operacionesDetalle.Any())
                throw new DomainException("No se puede contabilizar una operación sin detalles.");

            Contabilizada = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void DesContabilizar(string modificadoPor)
        {
            if (!Contabilizada)
                throw new DomainException("La operación no está contabilizada.");

            Contabilizada = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("La operación ya está cancelada.");

            if (Contabilizada)
                throw new DomainException("No se puede cancelar una operación contabilizada. Debe descontabilizarla primero.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("La operación no está cancelada.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        public int GetCantidadDetalles()
        {
            return _operacionesDetalle.Count(d => !d.Cancelado);
        }

        public decimal GetTotalImporteCosto()
        {
            return _operacionesDetalle.Where(d => !d.Cancelado).Sum(d => d.ImporteCosto);
        }

        public decimal GetTotalImporteVenta()
        {
            return _operacionesDetalle.Where(d => !d.Cancelado).Sum(d => d.ImporteVenta);
        }
    }
}