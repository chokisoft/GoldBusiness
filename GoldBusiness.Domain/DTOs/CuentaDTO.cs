namespace GoldBusiness.Domain.DTOs
{
    public class CuentaDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int SubGrupoCuenta { get; set; }
        public string? SubGrupoCuentaDescripcion { get; set; }
        public int? GrupoCuenta { get; set; }
        public string? GrupoCuentaDescripcion { get; set; }
        public bool Cancelado { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string ModificadoPor { get; set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; set; }
    }
}