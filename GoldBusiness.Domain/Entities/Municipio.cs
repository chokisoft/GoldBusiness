using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;

namespace GoldBusiness.Domain.Entities
{
    public class Municipio : BaseEntity
    {
        private readonly HashSet<MunicipioTranslation> _translations = new();

        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int ProvinciaId { get; private set; }
        public bool Cancelado { get; private set; }

        // Navegación
        public Provincia Provincia { get; private set; } = null!;
        public IReadOnlyCollection<MunicipioTranslation> Translations => _translations;

        protected Municipio() { }

        public Municipio(
            string codigo,
            string descripcion,
            int provinciaId,
            string creadoPor)
        {
            SetCodigo(codigo);
            SetDescripcion(descripcion);
            SetProvinciaId(provinciaId);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (codigo.Length > 25)
                throw new DomainException("El código no puede exceder 25 caracteres.");

            Codigo = codigo.ToUpperInvariant();
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción es obligatoria.");

            if (descripcion.Length > 100)
                throw new DomainException("La descripción no puede exceder 100 caracteres.");

            Descripcion = descripcion.Trim();
        }

        public void SetProvinciaId(int provinciaId)
        {
            if (provinciaId <= 0)
                throw new DomainException("El ID de la provincia debe ser mayor que cero.");

            ProvinciaId = provinciaId;
        }

        public void Cancelar(string modificadoPor)
        {
            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Activar(string modificadoPor)
        {
            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void Actualizar(string descripcion, string modificadoPor)
        {
            SetDescripcion(descripcion);
            ActualizarAuditoria(modificadoPor);
        }
    }
}