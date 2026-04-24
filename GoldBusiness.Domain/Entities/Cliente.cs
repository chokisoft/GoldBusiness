using GoldBusiness.Domain.Enums;
using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Helpers;
using GoldBusiness.Domain.Translation;
using System.Text.RegularExpressions;

namespace GoldBusiness.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        private readonly HashSet<ClienteTranslation> _translations = new();
        private readonly HashSet<CuentaCobrarPagar> _cuentasCobrarPagar = new();
        private readonly HashSet<OperacionesEncabezado> _operacionesEncabezado = new();

        // ═══════════════════════════════════════════════════════════════
        // 📋 PROPIEDADES PRINCIPALES
        // ═══════════════════════════════════════════════════════════════
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 🧾 DATOS FISCALES
        // ═══════════════════════════════════════════════════════════════
        public string IdentificadorFiscal { get; private set; } = string.Empty;
        public TipoIdentificacionFiscal? TipoIdentificadorFiscal { get; private set; }
        public RegimenFiscal? RegimenFiscal { get; private set; }
        public decimal TasaIva { get; private set; }
        public bool ExentoIva { get; private set; }
        public bool Extranjero { get; private set; }
        public string CodigoPaisIso { get; private set; } = string.Empty;
        public bool ValidarIdentificadorFiscal { get; private set; }
        public bool InversionSujetoPasivo { get; private set; }

        // ═══════════════════════════════════════════════════════════════
        // 🏦 DATOS BANCARIOS
        // ═══════════════════════════════════════════════════════════════
        public string Iban { get; private set; } = string.Empty;
        public string BicoSwift { get; private set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 📍 LOCALIZACIÓN
        // ═══════════════════════════════════════════════════════════════
        public string Direccion { get; private set; } = string.Empty;
        public int? PaisId { get; private set; }
        public int? ProvinciaId { get; private set; }
        public int? MunicipioId { get; private set; }
        public int? CodigoPostalId { get; private set; }

        // ═══════════════════════════════════════════════════════════════
        // 📞 CONTACTO
        // ═══════════════════════════════════════════════════════════════
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public string Web { get; private set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 🔧 CONTROL
        // ═══════════════════════════════════════════════════════════════
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Pais? Pais { get; private set; }
        public Provincia? Provincia { get; private set; }
        public Municipio? Municipio { get; private set; }
        public CodigoPostal? CodigoPostal { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ClienteTranslation> Translations => _translations;
        public IReadOnlyCollection<CuentaCobrarPagar> CuentasCobrarPagar => _cuentasCobrarPagar;
        public IReadOnlyCollection<OperacionesEncabezado> OperacionesEncabezado => _operacionesEncabezado;

        // Constructor protegido para EF Core
        protected Cliente() { }

        // Constructor con validaciones
        public Cliente(
            string codigo,
            string descripcion,
            string? identificadorFiscal,
            string? iban,
            string? bicoSwift,
            decimal tasaIva,
            string? direccion,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            string? web,
            string? email,
            string? telefono,
            TipoIdentificacionFiscal? tipoIdentificadorFiscal,
            RegimenFiscal? regimenFiscal,
            bool exentoIva,
            bool extranjero,
            string? codigoPaisIso,
            bool validarIdentificadorFiscal,
            bool inversionSujetoPasivo,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetIdentificadorFiscal(identificadorFiscal ?? string.Empty);
            SetIban(iban ?? string.Empty);
            SetBicoSwift(bicoSwift ?? string.Empty);
            SetTasaIva(tasaIva);
            SetDireccion(direccion ?? string.Empty);
            PaisId = paisId;
            ProvinciaId = provinciaId;
            MunicipioId = municipioId;
            CodigoPostalId = codigoPostalId;
            SetWeb(web ?? string.Empty);
            SetEmail(email ?? string.Empty);
            SetTelefono(telefono ?? string.Empty, null);
            SetTipoIdentificadorFiscal(tipoIdentificadorFiscal);
            SetRegimenFiscal(regimenFiscal);
            SetExentoIva(exentoIva);
            SetExtranjero(extranjero);
            SetCodigoPaisIso(codigoPaisIso ?? string.Empty);
            SetValidarIdentificadorFiscal(validarIdentificadorFiscal);
            SetInversionSujetoPasivo(inversionSujetoPasivo);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 8)
                throw new DomainException("El código debe tener exactamente 8 caracteres.");

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

        public void SetIdentificadorFiscal(string identificadorFiscal)
        {
            if (!string.IsNullOrWhiteSpace(identificadorFiscal) && identificadorFiscal.Length > 30)
                throw new DomainException("El identificador fiscal no puede exceder 30 caracteres.");

            IdentificadorFiscal = identificadorFiscal?.Trim().ToUpperInvariant() ?? string.Empty;
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

        public void SetTasaIva(decimal tasaIva)
        {
            if (tasaIva < -0.01m || tasaIva > 99.99m)
                throw new DomainException("La tasa de IVA debe estar entre -0.01 y 99.99.");

            TasaIva = tasaIva;
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

        public void SetEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 256)
                    throw new DomainException("El email no puede exceder 256 caracteres.");

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainException("El email no es válido.");
            }

            Email = email?.Trim() ?? string.Empty;
        }

        public void SetTelefono(string telefono, Pais? pais)
        {
            if (!string.IsNullOrWhiteSpace(telefono))
            {
                if (telefono.Length > 50)
                    throw new DomainException("El teléfono no puede exceder 50 caracteres.");

                ValidarTelefonoConPais(telefono, "teléfono", pais);
            }

            Telefono = telefono?.Trim() ?? string.Empty;
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

        public void SetTipoIdentificadorFiscal(TipoIdentificacionFiscal? tipo)
        {
            if (tipo.HasValue && !Enum.IsDefined(typeof(TipoIdentificacionFiscal), tipo.Value))
                throw new DomainException("Tipo de identificador fiscal inválido.");

            TipoIdentificadorFiscal = tipo;
        }

        public void SetRegimenFiscal(RegimenFiscal? regimen)
        {
            if (regimen.HasValue && !Enum.IsDefined(typeof(RegimenFiscal), regimen.Value))
                throw new DomainException("Régimen fiscal inválido.");

            RegimenFiscal = regimen;
        }

        public void SetExentoIva(bool exento)
        {
            ExentoIva = exento;
        }

        public void SetExtranjero(bool extranjero)
        {
            Extranjero = extranjero;
        }

        public void SetCodigoPaisIso(string codigo)
        {
            if (!string.IsNullOrWhiteSpace(codigo) && codigo.Length > 3)
                throw new DomainException("El código ISO de país no puede exceder 3 caracteres.");

            CodigoPaisIso = codigo?.Trim().ToUpperInvariant() ?? string.Empty;
        }

        public void SetValidarIdentificadorFiscal(bool validar)
        {
            ValidarIdentificadorFiscal = validar;
        }

        public void SetInversionSujetoPasivo(bool inversion)
        {
            InversionSujetoPasivo = inversion;
        }

        public void Actualizar(
            string descripcion,
            string? identificadorFiscal,
            string? iban,
            string? bicoSwift,
            decimal tasaIva,
            string? direccion,
            int? paisId,
            int? provinciaId,
            int? municipioId,
            int? codigoPostalId,
            string? web,
            string? email,
            string? telefono,
            TipoIdentificacionFiscal? tipoIdentificadorFiscal,
            RegimenFiscal? regimenFiscal,
            bool exentoIva,
            bool extranjero,
            string? codigoPaisIso,
            bool validarIdentificadorFiscal,
            bool inversionSujetoPasivo,
            Pais? pais,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            SetIdentificadorFiscal(identificadorFiscal ?? string.Empty);
            SetIban(iban ?? string.Empty);
            SetBicoSwift(bicoSwift ?? string.Empty);
            SetTasaIva(tasaIva);
            SetDireccion(direccion ?? string.Empty);
            SetUbicacion(paisId, provinciaId, municipioId, codigoPostalId);
            SetWeb(web ?? string.Empty);
            SetEmail(email ?? string.Empty);
            SetTelefono(telefono ?? string.Empty, pais);
            SetTipoIdentificadorFiscal(tipoIdentificadorFiscal);
            SetRegimenFiscal(regimenFiscal);
            SetExentoIva(exentoIva);
            SetExtranjero(extranjero);
            SetCodigoPaisIso(codigoPaisIso ?? string.Empty);
            SetValidarIdentificadorFiscal(validarIdentificadorFiscal);
            SetInversionSujetoPasivo(inversionSujetoPasivo);
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
                _translations.Add(new ClienteTranslation(Id, lang, descripcion, usuario));
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