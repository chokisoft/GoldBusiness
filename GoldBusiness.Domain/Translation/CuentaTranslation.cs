using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class CuentaTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int CuentaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public Cuenta Cuenta { get; private set; } = null!;

        // Constructor protegido para EF Core
        protected CuentaTranslation() { }

        // Constructor con validaciones
        public CuentaTranslation(int cuentaId, string language, string descripcion, string creadoPor)
        {
            CuentaId = cuentaId;
            EstablecerIdioma(language);
            SetDescripcion(descripcion, creadoPor); // ✅ Usar el método con validaciones
            EstablecerCreador(creadoPor);
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
    }
}