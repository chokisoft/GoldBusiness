using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Helpers;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class SystemConfiguration : BaseEntity
    {
        private readonly HashSet<SystemConfigurationTranslation> _translations = new();
        private readonly HashSet<Establecimiento> _establecimiento = new();

        public int Id { get; private set; }
        public string CodigoSistema { get; private set; } = string.Empty;
        public string Licencia { get; private set; } = string.Empty;
        public string NombreNegocio { get; private set; } = string.Empty;
        public string Direccion { get; private set; } = string.Empty;

        // Localización dependiente
        public int PaisId { get; private set; }
        public Pais Pais { get; private set; } = null!;

        public int ProvinciaId { get; private set; }
        public Provincia Provincia { get; private set; } = null!;

        public int MunicipioId { get; private set; }
        public Municipio Municipio { get; private set; } = null!;

        public int CodigoPostalId { get; private set; }
        public CodigoPostal CodigoPostal { get; private set; } = null!;

        public string Imagen { get; private set; } = string.Empty;
        public string Web { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;

        public int? CuentaPagarId { get; private set; }
        public int? CuentaCobrarId { get; private set; }

        public DateTime Caducidad { get; private set; }

        public Cuenta? CuentaCobrar { get; private set; }
        public Cuenta? CuentaPagar { get; private set; }

        public IReadOnlyCollection<SystemConfigurationTranslation> Translations => _translations;
        public IReadOnlyCollection<Establecimiento> Establecimiento => _establecimiento;

        protected SystemConfiguration() { }

        // Constructor actualizado para localización dependiente
        public SystemConfiguration(
            string codigoSistema,
            string licencia,
            string nombreNegocio,
            string? direccion,
            int paisId,
            int provinciaId,
            int municipioId,
            int codigoPostalId,
            string? imagen,
            string? web,
            string? email,
            string? telefono,
            DateTime caducidad,
            string creadoPor)
        {
            SetCodigoSistema(codigoSistema);
            SetLicencia(licencia);
            SetNombreNegocio(nombreNegocio);
            SetDireccion(direccion ?? string.Empty);
            SetPais(paisId);
            SetProvincia(provinciaId);
            SetMunicipio(municipioId);
            SetCodigoPostal(codigoPostalId);
            SetImagen(imagen ?? string.Empty);
            SetWeb(web ?? string.Empty);
            SetEmail(email ?? string.Empty);
            SetTelefono(telefono ?? string.Empty);
            SetCaducidad(caducidad);
            EstablecerCreador(creadoPor);
        }

        // Métodos de dominio para cambiar dependencias
        public void SetPais(int paisId)
        {
            PaisId = paisId;
            ProvinciaId = 0;
            MunicipioId = 0;
            CodigoPostalId = 0;
        }

        public void SetProvincia(int provinciaId)
        {
            ProvinciaId = provinciaId;
            MunicipioId = 0;
            CodigoPostalId = 0;
        }

        public void SetMunicipio(int municipioId)
        {
            MunicipioId = municipioId;
            CodigoPostalId = 0;
        }

        public void SetCodigoPostal(int codigoPostalId)
        {
            CodigoPostalId = codigoPostalId;
        }

        // Métodos de validación y actualización (sin cambios lógicos)
        public void SetCodigoSistema(string codigoSistema)
        {
            if (string.IsNullOrWhiteSpace(codigoSistema))
                throw new DomainException("El código del sistema es obligatorio.");

            if (codigoSistema.Length > 50)
                throw new DomainException("El código del sistema no puede exceder 50 caracteres.");

            CodigoSistema = codigoSistema.Trim();
        }

        public void SetLicencia(string licencia)
        {
            if (string.IsNullOrWhiteSpace(licencia))
                throw new DomainException("La licencia es obligatoria.");

            if (licencia.Length > 100)
                throw new DomainException("La licencia no puede exceder 100 caracteres.");

            Licencia = licencia.Trim();
        }

        public void SetNombreNegocio(string nombreNegocio)
        {
            if (string.IsNullOrWhiteSpace(nombreNegocio))
                throw new DomainException("El nombre del negocio es obligatorio.");

            if (nombreNegocio.Length > 256)
                throw new DomainException("El nombre del negocio no puede exceder 256 caracteres.");

            NombreNegocio = nombreNegocio.Trim();
        }

        public void SetDireccion(string direccion)
        {
            if (!string.IsNullOrWhiteSpace(direccion) && direccion.Length > 512)
                throw new DomainException("La dirección no puede exceder 512 caracteres.");

            Direccion = direccion?.Trim() ?? string.Empty;
        }

        public void SetImagen(string imagen)
        {
            if (!string.IsNullOrWhiteSpace(imagen))
            {
                if (imagen.Length > 500)
                    throw new DomainException("El nombre de archivo del logo no puede exceder 500 caracteres.");

                if (imagen.Contains("://"))
                {
                    if (!Uri.TryCreate(imagen, UriKind.Absolute, out var uri) ||
                        (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                    {
                        throw new DomainException("La URL de la imagen no es válida.");
                    }
                }
                else
                {
                    var invalidChars = Path.GetInvalidFileNameChars();
                    if (imagen.Any(c => invalidChars.Contains(c)))
                    {
                        throw new DomainException("El nombre de archivo contiene caracteres no válidos.");
                    }

                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif", ".webp" };
                    var extension = Path.GetExtension(imagen).ToLowerInvariant();

                    if (!string.IsNullOrEmpty(extension) && !allowedExtensions.Contains(extension))
                    {
                        throw new DomainException("La extensión del archivo no es válida. Use PNG, JPG, GIF o WEBP.");
                    }
                }
            }

            Imagen = imagen?.Trim() ?? string.Empty;
        }

        public void SetWeb(string web)
        {
            if (!string.IsNullOrWhiteSpace(web))
            {
                if (web.Length > 256)
                    throw new DomainException("La URL del sitio web no puede exceder 256 caracteres.");

                if (!Uri.TryCreate(web, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    throw new DomainException("La URL del sitio web no es válida.");
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

                if (!System.Text.RegularExpressions.Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new DomainException("El email no es válido.");
                }
            }

            Email = email?.Trim() ?? string.Empty;
        }

        public void SetTelefono(string telefono)
        {
            if (!string.IsNullOrWhiteSpace(telefono))
            {
                if (telefono.Length > 20)
                    throw new DomainException("El teléfono no puede exceder 20 caracteres.");

                var telefonoLimpio = telefono.TrimStart().StartsWith("+")
                    ? "+" + System.Text.RegularExpressions.Regex.Replace(telefono, @"[^\d]", "")
                    : System.Text.RegularExpressions.Regex.Replace(telefono, @"[^\d]", "");

                var cubaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?53)?[2-9]\d{7}$");
                var espanaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?34)?[6-9]\d{8}$");
                var usaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?1)?[2-9]\d{9}$");
                var franciaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?33)?[1-9]\d{8}$");

                var esValido = cubaRegex.IsMatch(telefonoLimpio) ||
                              espanaRegex.IsMatch(telefonoLimpio) ||
                              usaRegex.IsMatch(telefonoLimpio) ||
                              franciaRegex.IsMatch(telefonoLimpio);

                if (!esValido)
                {
                    throw new DomainException(
                        "El teléfono no tiene un formato válido. " +
                        "Formatos aceptados: Cuba (+53 8 dígitos), España (+34 9 dígitos), " +
                        "EE.UU. (+1 10 dígitos), Francia (+33 10 dígitos).");
                }
            }

            Telefono = telefono?.Trim() ?? string.Empty;
        }

        public void SetCaducidad(DateTime caducidad)
        {
            if (caducidad == default)
                throw new DomainException("La fecha de caducidad es obligatoria.");

            Caducidad = caducidad;
        }

        public void SetCuentas(int? cuentaPagarId, int? cuentaCobrarId)
        {
            CuentaPagarId = cuentaPagarId;
            CuentaCobrarId = cuentaCobrarId;
        }

        // 🌍 MÉTODOS DE TRADUCCIÓN
        public void AddOrUpdateTranslation(
            string language,
            string nombreNegocio,
            string direccion,
            string municipio,
            string provincia,
            string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.SetNombreNegocio(nombreNegocio, usuario);
                existing.SetDireccion(direccion, usuario);
                existing.SetMunicipio(municipio, usuario);
                existing.SetProvincia(provincia, usuario);
            }
            else
            {
                _translations.Add(new SystemConfigurationTranslation(
                    Id, lang, nombreNegocio, direccion, municipio, provincia, usuario));
            }
        }

        public string GetNombreNegocio(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.NombreNegocio))
                return match.NombreNegocio;

            if (!string.IsNullOrWhiteSpace(NombreNegocio))
                return NombreNegocio;

            var fallbackMatch = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.NombreNegocio;

            return string.Empty;
        }

        public string GetDireccion(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Direccion))
                return match.Direccion;

            if (!string.IsNullOrWhiteSpace(Direccion))
                return Direccion;

            var fallbackMatch = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Direccion;

            return string.Empty;
        }

        public string GetMunicipio(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Direccion))
                return match.Municipio;

            if (Municipio != null && !string.IsNullOrWhiteSpace(Municipio.Descripcion))
                return Municipio.Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Municipio;

            return string.Empty;
        }

        public string GetProvincia(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Provincia))
                return match.Provincia;

            if (Provincia != null && !string.IsNullOrWhiteSpace(Provincia.Descripcion))
                return Provincia.Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Provincia;

            return string.Empty;
        }

        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // Ahora Update trabaja con IDs para evitar conversiones incorrectas
        public void Update(
            string nombreNegocio,
            string direccion,
            int provinciaId,
            int municipioId,
            int codigoPostalId,
            string web,
            string email,
            string telefono,
            int? cuentaPagarId,
            int? cuentaCobrarId,
            DateTime caducidad,
            string modificadoPor)
        {
            SetNombreNegocio(nombreNegocio);
            SetDireccion(direccion);
            SetProvincia(provinciaId);
            SetMunicipio(municipioId);
            SetCodigoPostal(codigoPostalId);
            SetWeb(web);
            SetEmail(email);
            SetTelefono(telefono);
            SetCuentas(cuentaPagarId, cuentaCobrarId);
            SetCaducidad(caducidad);
            ActualizarAuditoria(modificadoPor);
        }

        public void RenovarLicencia(string nuevaLicencia, DateTime nuevaCaducidad, string modificadoPor)
        {
            SetLicencia(nuevaLicencia);
            SetCaducidad(nuevaCaducidad);
            ActualizarAuditoria(modificadoPor);
        }

        // 📊 MÉTODOS DE CONSULTA
        public bool EstaVigente() => Caducidad > DateTime.UtcNow;
        public bool EstaProximoAVencer(int diasAnticipacion = 30) => Caducidad <= DateTime.UtcNow.AddDays(diasAnticipacion) && Caducidad > DateTime.UtcNow;
        public bool EstaVencida() => Caducidad <= DateTime.UtcNow;
        public int DiasRestantes() => (Caducidad - DateTime.UtcNow).Days > 0 ? (Caducidad - DateTime.UtcNow).Days : 0;
        public bool TieneImagen() => !string.IsNullOrEmpty(Imagen);

        // Método para verificar si tiene cuentas configuradas
        public bool TieneCuentasConfiguradas() => CuentaPagarId.HasValue && CuentaCobrarId.HasValue;
    }
}