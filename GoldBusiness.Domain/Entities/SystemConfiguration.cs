using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class SystemConfiguration
    {
        private readonly HashSet<SystemConfigurationTranslation> _translations = new();
        private readonly HashSet<Establecimiento> _establecimientos = new();

        public int Id { get; private set; }
        public string CodigoSistema { get; private set; } = string.Empty;
        public string Licencia { get; private set; } = string.Empty;
        public string NombreNegocio { get; private set; } = string.Empty;
        public string Direccion { get; private set; } = string.Empty;
        public string Municipio { get; private set; } = string.Empty;
        public string Provincia { get; private set; } = string.Empty;
        public string CodPostal { get; private set; } = string.Empty;
        public byte[] Imagen { get; private set; } = Array.Empty<byte>();
        public string Web { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public int CuentaPagarId { get; private set; }
        public int CuentaCobrarId { get; private set; }
        public DateTime Caducidad { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        // Propiedades de navegación
        public Cuenta CuentaCobrarNavigation { get; private set; } = null!;
        public Cuenta CuentaPagarNavigation { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<SystemConfigurationTranslation> Translations => _translations;
        public IReadOnlyCollection<Establecimiento> Establecimientos => _establecimientos;

        // Constructor protegido para EF Core
        protected SystemConfiguration() { }

        // Constructor con validaciones
        public SystemConfiguration(
            string codigoSistema,
            string licencia,
            string nombreNegocio,
            int cuentaPagarId,
            int cuentaCobrarId,
            DateTime caducidad,
            string creadoPor)
        {
            SetCodigoSistema(codigoSistema);
            SetLicencia(licencia);
            SetNombreNegocio(nombreNegocio);
            CuentaPagarId = cuentaPagarId;
            CuentaCobrarId = cuentaCobrarId;
            SetCaducidad(caducidad);

            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
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

        public void SetImagen(byte[] imagen)
        {
            Imagen = imagen ?? Array.Empty<byte>();
        }

        public void SetWeb(string web)
        {
            if (!string.IsNullOrWhiteSpace(web))
            {
                if (web.Length > 256)
                    throw new DomainException("La URL del sitio web no puede exceder 256 caracteres.");

                // Validación básica de URL
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

                // Validación básica de email
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

                // Validación básica de teléfono (formato: (####) ####-#### o ####-####)
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

        public void SetCuentas(int cuentaPagarId, int cuentaCobrarId)
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
            var lang = NormalizeLang(language);
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
            var lang = NormalizeLang(language);
            var fb = NormalizeLang(fallback);

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
            var lang = NormalizeLang(language);
            var fb = NormalizeLang(fallback);

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

        public void Update(
            string nombreNegocio,
            string direccion,
            string municipio,
            string provincia,
            string codPostal,
            string web,
            string email,
            string telefono,
            int cuentaPagarId,
            int cuentaCobrarId,
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

        public bool EstaVigente()
        {
            return Caducidad > DateTime.UtcNow;
        }

        public bool EstaProximoAVencer(int diasAnticipacion = 30)
        {
            return Caducidad <= DateTime.UtcNow.AddDays(diasAnticipacion) && Caducidad > DateTime.UtcNow;
        }

        public bool EstaVencida()
        {
            return Caducidad <= DateTime.UtcNow;
        }

        public int DiasRestantes()
        {
            var dias = (Caducidad - DateTime.UtcNow).Days;
            return dias > 0 ? dias : 0;
        }

        public bool TieneImagen()
        {
            return Imagen != null && Imagen.Length > 0;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private void ActualizarAuditoria(string usuario)
        {
            ModificadoPor = usuario ?? throw new ArgumentNullException(nameof(usuario));
            FechaHoraModificado = DateTime.UtcNow;
        }

        private static string NormalizeLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang)) return "es";
            var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return parts[0].ToLowerInvariant();
        }
    }
}