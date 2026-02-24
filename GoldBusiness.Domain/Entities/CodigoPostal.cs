using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;
using System.Linq;

namespace GoldBusiness.Domain.Entities
{
    public class CodigoPostal : BaseEntity
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public int MunicipioId { get; private set; }
        public bool Cancelado { get; private set; }

        public Municipio Municipio { get; private set; } = null!;

        protected CodigoPostal() { }

        public CodigoPostal(string codigo, int municipioId, string creadoPor)
        {
            SetCodigo(codigo);
            SetMunicipioId(municipioId);
            EstablecerCreador(creadoPor);
            Cancelado = false;
        }

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código postal es obligatorio.");
            if (codigo.Length > 20)
                throw new DomainException("El código postal no puede exceder 20 caracteres.");
            Codigo = codigo.Trim();
        }

        public void SetMunicipioId(int municipioId)
        {
            if (municipioId <= 0)
                throw new DomainException("El ID de municipio debe ser mayor que cero.");
            MunicipioId = municipioId;
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

        public void Actualizar(string codigo, int municipioId, string modificadoPor)
        {
            SetCodigo(codigo);
            SetMunicipioId(municipioId);
            ActualizarAuditoria(modificadoPor);
        }
    }
}