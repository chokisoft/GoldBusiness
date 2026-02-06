using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Translation
{
    public class OperacionesEncabezadoTranslation : BaseTranslation
    {
        public int Id { get; private set; }
        public int OperacionesEncabezadoId { get; private set; }
        public string Concepto { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;

        public OperacionesEncabezado OperacionesEncabezado { get; private set; } = null!;

        protected OperacionesEncabezadoTranslation() { }

        public OperacionesEncabezadoTranslation(
            int operacionesEncabezadoId,
            string language,
            string concepto,
            string observaciones,
            string creadoPor)
        {
            OperacionesEncabezadoId = operacionesEncabezadoId;
            EstablecerIdioma(language);
            SetConcepto(concepto, creadoPor);
            SetObservaciones(observaciones, creadoPor);
            EstablecerCreador(creadoPor);
        }

        public void SetConcepto(string concepto, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(concepto) && concepto.Length > 512)
                throw new DomainException("El concepto no puede exceder 512 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Concepto = concepto?.Trim() ?? string.Empty;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }

        public void SetObservaciones(string observaciones, string modificadoPor)
        {
            if (!string.IsNullOrWhiteSpace(observaciones) && observaciones.Length > 1024)
                throw new DomainException("Las observaciones no pueden exceder 1024 caracteres.");

            if (string.IsNullOrWhiteSpace(modificadoPor))
                throw new ArgumentNullException(nameof(modificadoPor));

            Observaciones = observaciones?.Trim() ?? string.Empty;
            ModificadoPor = modificadoPor;
            FechaHoraModificado = DateTime.UtcNow;
        }
    }
}