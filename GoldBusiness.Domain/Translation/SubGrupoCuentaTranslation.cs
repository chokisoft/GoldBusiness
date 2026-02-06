using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;
using System;

namespace GoldBusiness.Domain.Translation
{
    public class SubGrupoCuentaTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int SubGrupoCuentaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;

        public SubGrupoCuenta SubGrupoCuenta { get; private set; } = null!;

        protected SubGrupoCuentaTranslation() { }

        public SubGrupoCuentaTranslation(int subGrupoCuentaId, string language, string descripcion, string creadoPor)
        {
            SubGrupoCuentaId = subGrupoCuentaId;
            EstablecerIdioma(language);
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            EstablecerCreador(creadoPor);
        }

        public void SetDescripcion(string descripcion, string? modificadoPor)
        {
            Descripcion = descripcion ?? Descripcion;
            ModificadoPor = modificadoPor ?? ModificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}