using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class CuentaTranslation
    {
        public int Id { get; private set; }
        public int CuentaId { get; private set; }
        public string Language { get; private set; } = string.Empty; // "es","en","fr"
        public string Descripcion { get; private set; } = string.Empty;

        public string CreadoPor { get; private set; } = string.Empty;
        public DateTime FechaHoraCreado { get; private set; }
        public string ModificadoPor { get; private set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; private set; }

        public Cuenta Cuenta { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected CuentaTranslation() { }

        // Constructor con validaciones
        public CuentaTranslation(int cuentaId, string language, string descripcion, string creadoPor)
        {
            CuentaId = cuentaId;
            Language = NormalizeLang(language);
            SetDescripcion(descripcion, creadoPor); // ✅ Usar el método con validaciones
            CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
            FechaHoraCreado = DateTime.UtcNow;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetDescripcion(string descripcion, string modificadoPor)
        {
            // ✅ Validación: no puede ser null o vacío
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");
            
            // ✅ Validación: longitud máxima (256 caracteres según DbContext)
            if (descripcion.Length > 256)
                throw new DomainException("La descripción no puede exceder 256 caracteres.");
            
            // ✅ Validación: modificadoPor es requerido
            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));
            
            Descripcion = descripcion.Trim();
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS PRIVADOS
        // ═══════════════════════════════════════════════════════════════

        private static string NormalizeLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang)) return "es";
            var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return parts[0].ToLowerInvariant();
        }
    }
}