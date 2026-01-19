using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class Cliente
    {
        private readonly HashSet<ClienteTranslation> _translations = new();
        private readonly HashSet<CuentaCobrarPagar> _cuentasCobrarPagar = new();
        private readonly HashSet<OperacionesEncabezado> _operacionesEncabezado = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Nif { get; private set; } = string.Empty;
        public string Iban { get; private set; } = string.Empty;
        public string BicoSwift { get; private set; } = string.Empty;
        public decimal Iva { get; private set; }
        public string Direccion { get; private set; } = string.Empty;
        public string Municipio { get; private set; } = string.Empty;
        public string Provincia { get; private set; } = string.Empty;
        public string CodPostal { get; private set; } = string.Empty;
        public string Web { get; private set; } = string.Empty;
        public string Email1 { get; private set; } = string.Empty;
        public string Email2 { get; private set; } = string.Empty;
        public string Telefono1 { get; private set; } = string.Empty;
        public string Telefono2 { get; private set; } = string.Empty;
        public string Fax1 { get; private set; } = string.Empty;
        public string Fax2 { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ClienteTranslation> Translations => _translations;
        public IReadOnlyCollection<CuentaCobrarPagar> CuentasCobrarPagar => _cuentasCobrarPagar;
        public IReadOnlyCollection<OperacionesEncabezado> OperacionesEncabezado => _operacionesEncabezado;

        // Constructor protegido para EF Core
        protected Cliente() { }

        // Constructor con validaciones
        public Cliente(string codigo, string descripcion, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Cancelado = false;
            Iva = 0;
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

        public void SetMunicipio(string municipio)
        {
            if (!string.IsNullOrWhiteSpace(municipio) && municipio.Length > 50)
                throw new DomainException("El municipio no puede exceder 50 caracteres.");

            Municipio = municipio?.Trim() ?? string.Empty;
        }

        public void SetProvincia(string provincia)
        {
            if (!string.IsNullOrWhiteSpace(provincia) && provincia.Length > 50)
                throw new DomainException("La provincia no puede exceder 50 caracteres.");

            Provincia = provincia?.Trim() ?? string.Empty;
        }

        public void SetCodPostal(string codPostal)
        {
            if (!string.IsNullOrWhiteSpace(codPostal) && codPostal.Length > 5)
                throw new DomainException("El código postal no puede exceder 5 caracteres.");

            CodPostal = codPostal?.Trim() ?? string.Empty;
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

                if (!System.Text.RegularExpressions.Regex.IsMatch(email1, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainException("El email 1 no es válido.");
            }

            if (!string.IsNullOrWhiteSpace(email2))
            {
                if (email2.Length > 256)
                    throw new DomainException("El email 2 no puede exceder 256 caracteres.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(email2, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainException("El email 2 no es válido.");
            }

            Email1 = email1?.Trim() ?? string.Empty;
            Email2 = email2?.Trim() ?? string.Empty;
        }

        public void SetTelefonos(string telefono1, string telefono2 = "")
        {
            if (!string.IsNullOrWhiteSpace(telefono1))
            {
                if (telefono1.Length > 50)
                    throw new DomainException("El teléfono 1 no puede exceder 50 caracteres.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(telefono1, @"^\d{4}-\d{4}$"))
                    throw new DomainException("El teléfono 1 debe tener el formato ####-####.");
            }

            if (!string.IsNullOrWhiteSpace(telefono2))
            {
                if (telefono2.Length > 50)
                    throw new DomainException("El teléfono 2 no puede exceder 50 caracteres.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(telefono2, @"^\d{4}-\d{4}$"))
                    throw new DomainException("El teléfono 2 debe tener el formato ####-####.");
            }

            Telefono1 = telefono1?.Trim() ?? string.Empty;
            Telefono2 = telefono2?.Trim() ?? string.Empty;
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

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string descripcion, string usuario)
        {
            var lang = NormalizeLang(language);
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
            var lang = NormalizeLang(language);
            var fb = NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            if (!string.IsNullOrWhiteSpace(Descripcion))
                return Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Descripcion;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(
            string descripcion,
            string nif,
            string iban,
            string bicoSwift,
            decimal iva,
            string direccion,
            string municipio,
            string provincia,
            string codPostal,
            string web,
            string email1,
            string email2,
            string telefono1,
            string telefono2,
            string fax1,
            string fax2,
            string modificadoPor)
        {
            SetDescripcion(descripcion);
            SetNif(nif);
            SetIban(iban);
            SetBicoSwift(bicoSwift);
            SetIva(iva);
            SetDireccion(direccion);
            SetMunicipio(municipio);
            SetProvincia(provincia);
            SetCodPostal(codPostal);
            SetWeb(web);
            SetEmails(email1, email2);
            SetTelefonos(telefono1, telefono2);
            SetFaxes(fax1, fax2);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El cliente ya está cancelado.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El cliente no está cancelado.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public string GetCodigoDescripcion()
        {
            return $"{Codigo} | {Descripcion}";
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