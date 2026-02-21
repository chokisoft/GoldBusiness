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
                    throw new DomainException("El nombre de archivo del logo no puede exceder 500 caracteres.");

                // ✅ VALIDACIÓN DUAL: Acepta nombres de archivo locales o URLs externas
                if (imagen.Contains("://"))
                {
                    // Es una URL - validar formato
                    if (!Uri.TryCreate(imagen, UriKind.Absolute, out var uri) ||
                        (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                    {
                        throw new DomainException("La URL de la imagen no es válida.");
                    }
                }
                else
                {
                    // Es un nombre de archivo local - validar caracteres y extensión
                    var invalidChars = Path.GetInvalidFileNameChars();
                    if (imagen.Any(c => invalidChars.Contains(c)))
                    {
                        throw new DomainException("El nombre de archivo contiene caracteres no válidos.");
                    }

                    // Validar extensión permitida
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

                // Limpiar el teléfono (solo dígitos, manteniendo el + inicial si existe)
                var telefonoLimpio = telefono.TrimStart().StartsWith("+")
                    ? "+" + System.Text.RegularExpressions.Regex.Replace(telefono, @"[^\d]", "")
                    : System.Text.RegularExpressions.Regex.Replace(telefono, @"[^\d]", "");

                // Validar formatos por país
                // Cuba: 8 dígitos (5XXX XXXX móvil o XX XXXXXX fijo), con o sin +53
                var cubaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?53)?[2-9]\d{7}$");

                // España: 9 dígitos, con o sin +34
                var espanaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?34)?[6-9]\d{8}$");

                // EE.UU.: 10 dígitos, con o sin +1
                var usaRegex = new System.Text.RegularExpressions.Regex(@"^(?:\+?1)?[2-9]\d{9}$");

                // Francia: 10 dígitos, con o sin +33
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
                existing.SetMunicipio(direccion, usuario);
                existing.SetProvincia(direccion, usuario);
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

            if (!string.IsNullOrWhiteSpace(Municipio))
                return Municipio;

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

            if (!string.IsNullOrWhiteSpace(Provincia))
                return Provincia;

            var fallbackMatch = _translations.FirstOrDefault(t =>
                string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Provincia;

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