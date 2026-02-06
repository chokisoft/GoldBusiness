using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;
using System;

namespace GoldBusiness.Domain.Translation
{
    public class GrupoCuentaTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int GrupoCuentaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;


        public GrupoCuenta GrupoCuenta { get; private set; } = null!;

        protected GrupoCuentaTranslation() { }

        public GrupoCuentaTranslation(int grupoCuentaId, string language, string descripcion, string creadoPor)
        {
            GrupoCuentaId = grupoCuentaId;
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