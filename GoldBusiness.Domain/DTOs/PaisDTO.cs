namespace GoldBusiness.Domain.DTOs
{
    public record PaisDTO
    {
        public int Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public string CodigoAlpha2 { get; init; } = string.Empty;
        public string CodigoTelefono { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public string RegexTelefono { get; init; } = string.Empty;
        public string FormatoTelefono { get; init; } = string.Empty;
        public string FormatoEjemplo { get; init; } = string.Empty;
    }
}