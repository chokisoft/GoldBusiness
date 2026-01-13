using GoldBusiness.Domain.Exceptions;
using System.Linq;

namespace GoldBusiness.Domain.Entities
{
    public class GrupoCuenta
    {
        private readonly HashSet<SubGrupoCuenta> _subGrupoCuenta = new();
        private readonly HashSet<GrupoCuentaTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty; // opcional: valor por compatibilidad
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public IReadOnlyCollection<SubGrupoCuenta> SubGrupoCuenta => _subGrupoCuenta;
        public IReadOnlyCollection<GrupoCuentaTranslation> Translations => _translations;

        protected GrupoCuenta() { }

        public GrupoCuenta(string codigo, string descripcion, string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Cancelado = false;
        }

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 2 || !codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser un número de 2 dígitos.");
            Codigo = codigo;
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");
            Descripcion = descripcion;
        }

        // Traducciones: añadir/actualizar
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
                // Usar Id (ya debe existir si se llama tras AddAsync)
                _translations.Add(new GrupoCuentaTranslation(Id, lang, descripcion, usuario));
            }
        }

        // Obtener descripción localizada con fallback
        public string GetDescripcion(string language, string fallback = "es")
        {
            var lang = NormalizeLang(language);
            var fb = NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                return match.Descripcion;

            // fallback a descripción base
            if (!string.IsNullOrWhiteSpace(Descripcion))
                return Descripcion;

            // fallback a otro idioma (ej. español)
            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Descripcion;

            // último recurso: cadena vacía
            return string.Empty;
        }

        public void Update(string descripcion, string? modificadoPor)
        {
            SetDescripcion(descripcion);
            ModificadoPor = modificadoPor ?? ModificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        public void SoftDelete(string? modificadoPor)
        {
            Cancelado = true;
            ModificadoPor = modificadoPor ?? ModificadoPor;
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