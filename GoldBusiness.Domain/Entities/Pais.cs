using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Pais : BaseEntity
    {
        private readonly HashSet<PaisTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;          // ISO 3166-1 alpha-3: CUB, ESP, USA, FRA
        public string CodigoAlpha2 { get; private set; } = string.Empty;    // ISO 3166-1 alpha-2: CU, ES, US, FR
        public string CodigoTelefono { get; private set; } = string.Empty;  // +53, +34, +1, +33
        public string Descripcion { get; private set; } = string.Empty;
        public string RegexTelefono { get; private set; } = string.Empty;
        public string FormatoTelefono { get; private set; } = string.Empty; // "+53 XXXX-XXXX"
        public string FormatoEjemplo { get; private set; } = string.Empty;  // "+53 5234-5678"
        public bool Cancelado { get; private set; }

        // Colección de navegación (read-only)
        public IReadOnlyCollection<PaisTranslation> Translations => _translations;

        // Constructor protegido para EF Core
        protected Pais() { }

        // Constructor con validaciones (SIN nombreIngles)
        public Pais(
            string codigo,
            string codigoAlpha2,
            string codigoTelefono,
            string descripcion,
            string regexTelefono,
            string formatoTelefono,
            string formatoEjemplo,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetCodigoAlpha2(codigoAlpha2);
            SetCodigoTelefono(codigoTelefono);
            SetDescripcion(descripcion);
            SetRegexTelefono(regexTelefono);
            SetFormatoTelefono(formatoTelefono);
            SetFormatoEjemplo(formatoEjemplo);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 3)
                throw new DomainException("El código debe tener exactamente 3 caracteres (ISO 3166-1 alpha-3).");

            Codigo = codigo.ToUpperInvariant();
        }

        public void SetCodigoAlpha2(string codigoAlpha2)
        {
            if (string.IsNullOrWhiteSpace(codigoAlpha2) || codigoAlpha2.Length != 2)
                throw new DomainException("El código alpha-2 debe tener exactamente 2 caracteres (ISO 3166-1 alpha-2).");

            CodigoAlpha2 = codigoAlpha2.ToUpperInvariant();
        }

        public void SetCodigoTelefono(string codigoTelefono)
        {
            if (string.IsNullOrWhiteSpace(codigoTelefono))
                throw new DomainException("El código de teléfono es obligatorio.");

            if (!codigoTelefono.StartsWith("+"))
                throw new DomainException("El código de teléfono debe empezar con '+'.");

            if (codigoTelefono.Length > 5)
                throw new DomainException("El código de teléfono no puede exceder 5 caracteres.");

            CodigoTelefono = codigoTelefono.Trim();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("El nombre es obligatorio.");

            if (descripcion.Length > 100)
                throw new DomainException("El nombre no puede exceder 100 caracteres.");

            Descripcion = descripcion.Trim();
        }

        public void SetRegexTelefono(string regexTelefono)
        {
            if (string.IsNullOrWhiteSpace(regexTelefono))
                throw new DomainException("La expresión regular para teléfono es obligatoria.");

            if (regexTelefono.Length > 200)
                throw new DomainException("La expresión regular no puede exceder 200 caracteres.");

            RegexTelefono = regexTelefono.Trim();
        }

        public void SetFormatoTelefono(string formatoTelefono)
        {
            if (string.IsNullOrWhiteSpace(formatoTelefono))
                throw new DomainException("El formato de teléfono es obligatorio.");

            if (formatoTelefono.Length > 50)
                throw new DomainException("El formato de teléfono no puede exceder 50 caracteres.");

            FormatoTelefono = formatoTelefono.Trim();
        }

        public void SetFormatoEjemplo(string formatoEjemplo)
        {
            if (string.IsNullOrWhiteSpace(formatoEjemplo))
                throw new DomainException("El ejemplo de formato es obligatorio.");

            if (formatoEjemplo.Length > 20)
                throw new DomainException("El ejemplo de formato no puede exceder 20 caracteres.");

            FormatoEjemplo = formatoEjemplo.Trim();
        }

        public void Activar(string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Cancelado = true;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        public void Desactivar(string modificadoPor)
        {
            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Cancelado = false;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌐 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddTranslation(string language, string nombre, string creadoPor)
        {
            if (_translations.Any(t => t.Language == language))
                throw new DomainException($"Ya existe una traducción para el idioma '{language}'.");

            var translation = new PaisTranslation(Id, language, nombre, creadoPor);
            _translations.Add(translation);
        }

        public void UpdateTranslation(string language, string descripcion, string modificadoPor)
        {
            var translation = _translations.FirstOrDefault(t => t.Language == language);
            if (translation == null)
                throw new DomainException($"No existe una traducción para el idioma '{language}'.");

            translation.SetDescripcion(descripcion, modificadoPor);
        }

        public void RemoveTranslation(string language)
        {
            var translation = _translations.FirstOrDefault(t => t.Language == language);
            if (translation != null)
            {
                _translations.Remove(translation);
            }
        }

        public string GetNombre(string language)
        {
            var translation = _translations.FirstOrDefault(t => t.Language == language);
            return translation?.Descripcion ?? Descripcion;
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