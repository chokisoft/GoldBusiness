using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;
using System.Text.RegularExpressions;

namespace GoldBusiness.Domain.Entities
{
    public class Establecimiento : BaseEntity
    {
        private readonly HashSet<EstablecimientoTranslation> _translations = new();
        private readonly HashSet<Comprobante> _comprobantes = new();
        private readonly HashSet<ComprobanteTemporal> _comprobantesTemporales = new();
        private readonly HashSet<CuentaCobrarPagar> _cuentasCobrarPagar = new();
        private readonly HashSet<EstadoCuenta> _estadosCuenta = new();
        private readonly HashSet<Localidad> _localidades = new();
        private readonly HashSet<OperacionesEncabezado> _operacionesEncabezado = new();
        private readonly HashSet<Producto> _producto = new();

        public int Id { get; private set; }
        public int NegocioId { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Direccion { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public int? PaisId { get; private set; }
        public int? ProvinciaId { get; private set; }
        public int? MunicipioId { get; private set; }
        public int? CodigoPostalId { get; private set; }
        public bool Activo { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public SystemConfiguration Negocio { get; private set; } = null!;
        public Pais? Pais { get; private set; }
        public Provincia? Provincia { get; private set; }
        public Municipio? Municipio { get; private set; }
        public CodigoPostal? CodigoPostal { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<EstablecimientoTranslation> Translations => _translations;
        public IReadOnlyCollection<Comprobante> Comprobantes => _comprobantes;
        public IReadOnlyCollection<ComprobanteTemporal> ComprobantesTemporales => _comprobantesTemporales;
        public IReadOnlyCollection<CuentaCobrarPagar> CuentasCobrarPagar => _cuentasCobrarPagar;
        public IReadOnlyCollection<EstadoCuenta> EstadosCuenta => _estadosCuenta;
        public IReadOnlyCollection<Localidad> Localidades => _localidades;
        public IReadOnlyCollection<OperacionesEncabezado> OperacionesEncabezado => _operacionesEncabezado;
        public IReadOnlyCollection<Producto> Producto => _producto;

        // Constructor protegido para EF Core
        protected Establecimiento() { }

        // Constructor con validaciones
        public Establecimiento(
            string codigo, 
            string descripcion, 
            int negocioId, 
            string? direccion,
            string? telefono,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetDireccion(direccion ?? string.Empty);
            NegocioId = negocioId;
            PaisId = paisId;
            ProvinciaId = provinciaId;
            MunicipioId = municipioId;
            CodigoPostalId = codigoPostalId;
            SetTelefono(telefono ?? string.Empty, null);
            EstablecerCreador(creadoPor);
            Activo = true;
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 6)
                throw new DomainException("El código debe tener exactamente 6 caracteres.");

            Codigo = codigo.ToUpperInvariant();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 256)
                throw new DomainException("La descripción no puede exceder 256 caracteres.");

            Descripcion = descripcion.Trim();
        }

        public void SetDireccion(string direccion)
        {
            if (!string.IsNullOrWhiteSpace(direccion) && direccion.Length > 256)
                throw new DomainException("La dirección no puede exceder 256 caracteres.");

            Direccion = direccion?.Trim() ?? string.Empty;
        }

        public void SetTelefono(string telefono, Pais? pais)
        {
            if (string.IsNullOrWhiteSpace(telefono))
            {
                Telefono = string.Empty;
                return;
            }

            if (telefono.Length > 50)
                throw new DomainException("El teléfono no puede exceder 50 caracteres.");

            // Validar formato según país si está disponible
            if (pais != null && !string.IsNullOrWhiteSpace(pais.RegexTelefono))
            {
                var regex = new Regex(pais.RegexTelefono);
                if (!regex.IsMatch(telefono))
                    throw new DomainException($"El teléfono no cumple con el formato esperado. Ejemplo: {pais.FormatoEjemplo}");
            }

            Telefono = telefono.Trim();
        }

        public void SetUbicacion(int? paisId, int? provinciaId, int? municipioId, int? codigoPostalId)
        {
            PaisId = paisId;
            ProvinciaId = provinciaId;
            MunicipioId = municipioId;
            CodigoPostalId = codigoPostalId;
        }

        public void Actualizar(
            string descripcion,
            string? direccion,
            string? telefono,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            Pais? pais,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            SetDireccion(direccion ?? string.Empty);
            SetTelefono(telefono ?? string.Empty, pais);
            SetUbicacion(paisId, provinciaId, municipioId, codigoPostalId);
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string descripcion, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetDescripcion(descripcion, usuario);
            }
            else
            {
                _translations.Add(new EstablecimientoTranslation(Id, lang, descripcion, usuario));
            }
        }

        public string GetDescripcion(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            if (!string.IsNullOrWhiteSpace(Descripcion))
                return Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null && !string.IsNullOrWhiteSpace(fallbackMatch.Descripcion))
                return fallbackMatch.Descripcion;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(string descripcion, string modificadoPor)
        {
            SetDescripcion(descripcion);
            ActualizarAuditoria(modificadoPor);
        }

        public void Activar(string modificadoPor)
        {
            if (Activo)
                throw new DomainException("El establecimiento ya está activo.");

            Activo = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Desactivar(string modificadoPor)
        {
            if (!Activo)
                throw new DomainException("El establecimiento ya está desactivado.");

            Activo = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string descripcion, string modificadoPor)
        {
            SetDescripcion(descripcion);
            Cancelado = false;
            Activo = true;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public string GetCodigoDescripcion()
        {
            return $"{Codigo} | {Descripcion}";
        }

        public bool TieneProductosActivos()
        {
            return _producto.Any(p => !p.Cancelado);
        }

        public bool TieneLocalidadesActivas()
        {
            return _localidades.Any(l => !l.Cancelado);
        }

        public int GetCantidadProductos()
        {
            return _producto.Count(p => !p.Cancelado);
        }

        public int GetCantidadLocalidades()
        {
            return _localidades.Count(l => !l.Cancelado);
        }
    }
}