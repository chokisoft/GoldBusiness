using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

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
        public string Municipio { get; private set; } = string.Empty;
        public string Provincia { get; private set; } = string.Empty;
        public string CodPostal { get; private set; } = string.Empty;
        public string Imagen { get; private set; } = string.Empty;
        public string Web { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;

        // ✅ NULLABLE
        public int? CuentaPagarId { get; private set; }
        public int? CuentaCobrarId { get; private set; }

        public DateTime Caducidad { get; private set; }

        // ✅ Propiedades de navegación NULLABLE
        public Cuenta? CuentaCobrar { get; private set; }
        public Cuenta? CuentaPagar { get; private set; }

        public IReadOnlyCollection<SystemConfigurationTranslation> Translations => _translations;
        public IReadOnlyCollection<Establecimiento> Establecimiento => _establecimiento;

        protected SystemConfiguration() { }

        // ✅ CONSTRUCTOR SIN REQUERIR CUENTAS
        public SystemConfiguration(
            string codigoSistema,
            string licencia,
            string nombreNegocio,
            string? direccion,
            string? municipio,
            string? provincia,
            string? codPostal,
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
            SetMunicipio(municipio ?? string.Empty);
            SetProvincia(provincia ?? string.Empty);
            SetCodPostal(codPostal ?? string.Empty);
            SetImagen(imagen ?? string.Empty);
            SetWeb(web ?? string.Empty);
            SetEmail(email ?? string.Empty);
            SetTelefono(telefono ?? string.Empty);
            SetCaducidad(caducidad);
            EstablecerCreador(creadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

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

        public void SetMunicipio(string municipio)
        {
            if (!string.IsNullOrWhiteSpace(municipio) && municipio.Length > 128)
                throw new DomainException("El municipio no puede exceder 128 caracteres.");

            Municipio = municipio?.Trim() ?? string.Empty;
        }

        public void SetProvincia(string provincia)
        {
            if (!string.IsNullOrWhiteSpace(provincia) && provincia.Length > 128)
                throw new DomainException("La provincia no puede exceder 128 caracteres.");

            Provincia = provincia?.Trim() ?? string.Empty;
        }

        public void SetCodPostal(string codPostal)
        {
            if (!string.IsNullOrWhiteSpace(codPostal) && codPostal.Length > 20)
                throw new DomainException("El código postal no puede exceder 20 caracteres.");

            CodPostal = codPostal?.Trim() ?? string.Empty;
        }

        public void SetImagen(string imagen)
        {
            if (!string.IsNullOrWhiteSpace(imagen))
            {
                if (imagen.Length > 500)
                    throw new DomainException("La URL de la imagen no puede exceder 500 caracteres.");

                // Validación básica de URL
                if (!Uri.TryCreate(imagen, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    throw new DomainException("La URL de la imagen no es válida.");
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

                var telefonoLimpio = System.Text.RegularExpressions.Regex.Replace(telefono, @"[^\d]", "");
                if (telefonoLimpio.Length < 8)
                {
                    throw new DomainException("El teléfono debe tener al menos 8 dígitos.");
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

        // ✅ CAMBIADO: Acepta valores nullable
        public void SetCuentas(int? cuentaPagarId, int? cuentaCobrarId)
        {
            CuentaPagarId = cuentaPagarId;
            CuentaCobrarId = cuentaCobrarId;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(
            string language,
            string nombreNegocio,
            string direccion,
            string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.SetNombreNegocio(nombreNegocio, usuario);
                existing.SetDireccion(direccion, usuario);
            }
            else
            {
                _translations.Add(new SystemConfigurationTranslation(
                    Id, lang, nombreNegocio, direccion, usuario));
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

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        // ✅ CAMBIADO: Acepta cuentas nullable
        public void Update(
            string nombreNegocio,
            string direccion,
            string municipio,
            string provincia,
            string codPostal,
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
            SetMunicipio(municipio);
            SetProvincia(provincia);
            SetCodPostal(codPostal);
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

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public bool EstaVigente() => Caducidad > DateTime.UtcNow;
        public bool EstaProximoAVencer(int diasAnticipacion = 30) => Caducidad <= DateTime.UtcNow.AddDays(diasAnticipacion) && Caducidad > DateTime.UtcNow;
        public bool EstaVencida() => Caducidad <= DateTime.UtcNow;
        public int DiasRestantes() => (Caducidad - DateTime.UtcNow).Days > 0 ? (Caducidad - DateTime.UtcNow).Days : 0;
        public bool TieneImagen() => Imagen != null && Imagen.Length > 0;

        // ✅ NUEVO: Método para verificar si tiene cuentas configuradas
        public bool TieneCuentasConfiguradas() => CuentaPagarId.HasValue && CuentaCobrarId.HasValue;
    }
}