namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO de respuesta de autenticación con token JWT.
    /// Formato estandarizado con succeeded, message y data.
    /// </summary>
    public class AuthResponseDTO
    {
        /// <summary>
        /// Indica si la autenticación fue exitosa.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado de la operación.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos de autenticación cuando la operación es exitosa.
        /// </summary>
        public AuthDataDTO? Data { get; set; }
    }

    /// <summary>
    /// Datos de autenticación incluidos en la respuesta exitosa.
    /// </summary>
    public class AuthDataDTO
    {
        /// <summary>
        /// Token JWT para autenticación en solicitudes posteriores.
        /// Debe incluirse en el header Authorization como "Bearer {token}".
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token de actualización para renovar el token de acceso.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora UTC de expiración del token en formato ISO 8601.
        /// </summary>
        public string ExpiresAt { get; set; } = string.Empty;

        /// <summary>
        /// Información del usuario autenticado.
        /// </summary>
        public UserInfoDTO User { get; set; } = new UserInfoDTO();
    }

    /// <summary>
    /// Información básica del usuario autenticado.
    /// </summary>
    public class UserInfoDTO
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Roles asignados al usuario.
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
    }
}