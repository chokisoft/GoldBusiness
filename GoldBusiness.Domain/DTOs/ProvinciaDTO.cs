namespace GoldBusiness.Domain.DTOs
{
    public record ProvinciaDTO
    {
        public int Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public int PaisId { get; init; }
        public string PaisCodigo { get; init; } = string.Empty;
        public string PaisDescripcion { get; init; } = string.Empty;
    }
}