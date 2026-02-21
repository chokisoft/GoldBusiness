namespace GoldBusiness.Domain.DTOs
{
    public record MunicipioDTO
    {
        public int Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public int ProvinciaId { get; init; }
        public string ProvinciaCodigo { get; init; } = string.Empty;
        public string ProvinciaDescripcion { get; init; } = string.Empty;
    }
}