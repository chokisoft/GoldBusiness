namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO de respuesta de autenticación con token JWT.
    /// Contiene el token de acceso y la información del usuario autenticado.
    /// </summary>
    public class AuthResponseDTO
    {
        /// <summary>
        /// Token JWT para autenticación en solicitudes posteriores.
        /// Debe incluirse en el header Authorization como "Bearer {token}".
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora UTC de expiración del token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
