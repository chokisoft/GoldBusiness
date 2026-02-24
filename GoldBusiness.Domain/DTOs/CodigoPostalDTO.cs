namespace GoldBusiness.Domain.DTOs
{
    public record CodigoPostalDTO
    {
        public int Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public int MunicipioId { get; init; }
        public string MunicipioCodigo { get; init; } = string.Empty;
        public string MunicipioDescripcion { get; init; } = string.Empty;
        public bool Cancelado { get; init; }
        public string CreadoPor { get; init; } = string.Empty;
        public DateTime FechaHoraCreado { get; init; }
        public string ModificadoPor { get; init; } = string.Empty;
        public DateTime? FechaHoraModificado { get; init; }
    }
}