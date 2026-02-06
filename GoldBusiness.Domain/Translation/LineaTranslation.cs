using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class LineaTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int LineaId { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;


        public Linea Linea { get; private set; } = null!;

        protected LineaTranslation() { }

        public LineaTranslation(int lineaId, string language, string descripcion, string creadoPor)
        {
            LineaId = lineaId;
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