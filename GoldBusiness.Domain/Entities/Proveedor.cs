using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;
using System.Text.RegularExpressions;

namespace GoldBusiness.Domain.Entities
{
    public class Proveedor : BaseEntity
    {
        private readonly HashSet<ProveedorTranslation> _translations = new();
        private readonly HashSet<CuentaCobrarPagar> _cuentasCobrarPagar = new();
        private readonly HashSet<OperacionesEncabezado> _operacionesEncabezado = new();
        private readonly HashSet<Producto> _producto = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Nif { get; private set; } = string.Empty;
        public string Iban { get; private set; } = string.Empty;
        public string BicoSwift { get; private set; } = string.Empty;
        public decimal Iva { get; private set; }
        public string Direccion { get; private set; } = string.Empty;
        public string Telefono1 { get; private set; } = string.Empty;
        public string Telefono2 { get; private set; } = string.Empty;
        public int? PaisId { get; private set; }
        public int? ProvinciaId { get; private set; }
        public int? MunicipioId { get; private set; }
        public int? CodigoPostalId { get; private set; }
        public string Web { get; private set; } = string.Empty;
        public string Email1 { get; private set; } = string.Empty;
        public string Email2 { get; private set; } = string.Empty;
        public string Fax1 { get; private set; } = string.Empty;
        public string Fax2 { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Pais? Pais { get; private set; }
        public Provincia? Provincia { get; private set; }
        public Municipio? Municipio { get; private set; }
        public CodigoPostal? CodigoPostal { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ProveedorTranslation> Translations => _translations;
        public IReadOnlyCollection<CuentaCobrarPagar> CuentasCobrarPagar => _cuentasCobrarPagar;
        public IReadOnlyCollection<OperacionesEncabezado> OperacionesEncabezado => _operacionesEncabezado;
        public IReadOnlyCollection<Producto> Producto => _producto;

        // Constructor protegido para EF Core
        protected Proveedor() { }

        // Constructor con validaciones
        public Proveedor(
            string codigo,
            string descripcion,
            string? nif,
            string? iban,
            string? bicoSwift,
            decimal iva,
            string? direccion,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            string? web,
            string? email1,
            string? email2,
            string? telefono1,
            string? telefono2,
            string? fax1,
            string? fax2,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetNif(nif ?? string.Empty);
            SetIban(iban ?? string.Empty);
            SetBicoSwift(bicoSwift ?? string.Empty);
            SetIva(iva);
            SetDireccion(direccion ?? string.Empty);
            PaisId = paisId;
            ProvinciaId = provinciaId;
            MunicipioId = municipioId;
            CodigoPostalId = codigoPostalId;
            SetWeb(web ?? string.Empty);
            SetEmails(email1 ?? string.Empty, email2 ?? string.Empty);
            SetTelefonos(telefono1 ?? string.Empty, telefono2 ?? string.Empty, null);
            SetFaxes(fax1 ?? string.Empty, fax2 ?? string.Empty);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 5)
                throw new DomainException("El código debe tener exactamente 5 caracteres.");

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

        public void SetNif(string nif)
        {
            if (!string.IsNullOrWhiteSpace(nif) && nif.Length > 11)
                throw new DomainException("El NIF no puede exceder 11 caracteres.");

            Nif = nif?.Trim().ToUpperInvariant() ?? string.Empty;
        }

        public void SetIban(string iban)
        {
            if (!string.IsNullOrWhiteSpace(iban) && iban.Length > 27)
                throw new DomainException("El IBAN no puede exceder 27 caracteres.");

            Iban = iban?.Trim().ToUpperInvariant() ?? string.Empty;
        }

        public void SetBicoSwift(string bicoSwift)
        {
            if (!string.IsNullOrWhiteSpace(bicoSwift) && bicoSwift.Length > 11)
                throw new DomainException("El BIC/SWIFT no puede exceder 11 caracteres.");

            BicoSwift = bicoSwift?.Trim().ToUpperInvariant() ?? string.Empty;
        }

        public void SetIva(decimal iva)
        {
            if (iva < -0.01m || iva > 99.99m)
                throw new DomainException("El IVA debe estar entre -0.01 y 99.99.");

            Iva = iva;
        }

        public void SetDireccion(string direccion)
        {
            if (!string.IsNullOrWhiteSpace(direccion) && direccion.Length > 256)
                throw new DomainException("La dirección no puede exceder 256 caracteres.");

            Direccion = direccion?.Trim() ?? string.Empty;
        }

        public void SetUbicacion(int? paisId, int? provinciaId, int? municipioId, int? codigoPostalId)
        {
            PaisId = paisId;
            ProvinciaId = provinciaId;
            MunicipioId = municipioId;
            CodigoPostalId = codigoPostalId;
        }

        public void SetWeb(string web)
        {
            if (!string.IsNullOrWhiteSpace(web))
            {
                if (web.Length > 256)
                    throw new DomainException("La URL no puede exceder 256 caracteres.");

                if (!Uri.TryCreate(web, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    throw new DomainException("La URL no es válida.");
                }
            }

            Web = web?.Trim() ?? string.Empty;
        }

        public void SetEmails(string email1, string email2 = "")
        {
            if (!string.IsNullOrWhiteSpace(email1))
            {
                if (email1.Length > 256)
                    throw new DomainException("El email 1 no puede exceder 256 caracteres.");

                if (!Regex.IsMatch(email1, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainException("El email 1 no es válido.");
            }

            if (!string.IsNullOrWhiteSpace(email2))
            {
                if (email2.Length > 256)
                    throw new DomainException("El email 2 no puede exceder 256 caracteres.");

                if (!Regex.IsMatch(email2, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainException("El email 2 no es válido.");
            }

            Email1 = email1?.Trim() ?? string.Empty;
            Email2 = email2?.Trim() ?? string.Empty;
        }

        public void SetTelefonos(string telefono1, string telefono2, Pais? pais)
        {
            if (!string.IsNullOrWhiteSpace(telefono1))
            {
                if (telefono1.Length > 50)
                    throw new DomainException("El teléfono 1 no puede exceder 50 caracteres.");

                ValidarTelefonoConPais(telefono1, "teléfono 1", pais);
            }

            if (!string.IsNullOrWhiteSpace(telefono2))
            {
                if (telefono2.Length > 50)
                    throw new DomainException("El teléfono 2 no puede exceder 50 caracteres.");

                ValidarTelefonoConPais(telefono2, "teléfono 2", pais);
            }

            Telefono1 = telefono1?.Trim() ?? string.Empty;
            Telefono2 = telefono2?.Trim() ?? string.Empty;
        }

        private void ValidarTelefonoConPais(string telefono, string campo, Pais? pais)
        {
            // Validar formato según país si está disponible
            if (pais != null && !string.IsNullOrWhiteSpace(pais.RegexTelefono))
            {
                var regex = new Regex(pais.RegexTelefono);
                if (!regex.IsMatch(telefono))
                    throw new DomainException($"El {campo} no cumple con el formato esperado. Ejemplo: {pais.FormatoEjemplo}");
            }
        }

        public void SetFaxes(string fax1, string fax2 = "")
        {
            if (!string.IsNullOrWhiteSpace(fax1) && fax1.Length > 50)
                throw new DomainException("El fax 1 no puede exceder 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(fax2) && fax2.Length > 50)
                throw new DomainException("El fax 2 no puede exceder 50 caracteres.");

            Fax1 = fax1?.Trim() ?? string.Empty;
            Fax2 = fax2?.Trim() ?? string.Empty;
        }

        public void Actualizar(
            string descripcion,
            string? nif,
            string? iban,
            string? bicoSwift,
            decimal iva,
            string? direccion,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            string? web,
            string? email1,
            string? email2,
            string? telefono1,
            string? telefono2,
            string? fax1,
            string? fax2,
            Pais? pais,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            SetNif(nif ?? string.Empty);
            SetIban(iban ?? string.Empty);
            SetBicoSwift(bicoSwift ?? string.Empty);
            SetIva(iva);
            SetDireccion(direccion ?? string.Empty);
            SetUbicacion(paisId, provinciaId, municipioId, codigoPostalId);
            SetWeb(web ?? string.Empty);
            SetEmails(email1 ?? string.Empty, email2 ?? string.Empty);
            SetTelefonos(telefono1 ?? string.Empty, telefono2 ?? string.Empty, pais);
            SetFaxes(fax1 ?? string.Empty, fax2 ?? string.Empty);
            ActualizarAuditoria(modificadoPor);
        }

        public void Cancelar(string modificadoPor)
        {
            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Activar(string modificadoPor)
        {
            Cancelado = false;
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
                _translations.Add(new ProveedorTranslation(Id, lang, descripcion, usuario));
            }
        }

        public string GetDescripcion(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null && !string.IsNullOrWhiteSpace(fallbackMatch.Descripcion))
                return fallbackMatch.Descripcion;

            return Descripcion;
        }
    }
}