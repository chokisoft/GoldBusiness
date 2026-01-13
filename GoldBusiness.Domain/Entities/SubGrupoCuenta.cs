using GoldBusiness.Domain.Exceptions;
using System.Linq;

namespace GoldBusiness.Domain.Entities
{
    public class SubGrupoCuenta
    {
        private readonly HashSet<Cuenta> _cuenta = new();
        private readonly HashSet<SubGrupoCuentaTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public int GrupoCuentaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public bool Deudora { get; private set; }
        public bool Cancelado { get; private set; }
        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public GrupoCuenta GrupoCuenta { get; private set; } = null!;
        public IReadOnlyCollection<Cuenta> Cuenta => _cuenta;
        public IReadOnlyCollection<SubGrupoCuentaTranslation> Translations => _translations;

        protected SubGrupoCuenta() { }

        public SubGrupoCuenta(
            string codigo,
            int grupoCuentaId,
            string descripcion,
            bool deudora,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);

            GrupoCuentaId = grupoCuentaId;
            Deudora = deudora;
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
            Cancelado = false;
        }

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 5 || !codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser un número de 5 dígitos.");

            Codigo = codigo;
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            Descripcion = descripcion;
        }

        public void AgregarCuenta(Cuenta cuenta)
        {
            if (cuenta == null)
                throw new DomainException("La cuenta no puede ser nula.");

            _cuenta.Add(cuenta);
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
                _translations.Add(new SubGrupoCuentaTranslation(Id, lang, descripcion, usuario));
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

            if (!string.IsNullOrWhiteSpace(Descripcion))
                return Descripcion;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Descripcion;

            return string.Empty;
        }

        public void Update(string descripcion, int grupoCuentaId, bool deudora, string? modificadoPor)
        {
            SetDescripcion(descripcion);
            GrupoCuentaId = grupoCuentaId;
            Deudora = deudora;
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