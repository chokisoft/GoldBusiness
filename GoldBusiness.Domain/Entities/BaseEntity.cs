using GoldBusiness.Domain.Exceptions;

namespace GoldBusiness.Domain.Entities
{
    /// <summary>
    /// Clase base para entidades con auditoría.
    /// </summary>
    public abstract class BaseEntity
    {
        public string CreadoPor { get; protected set; } = string.Empty;
        public DateTime FechaHoraCreado { get; protected set; }
        public string ModificadoPor { get; protected set; } = string.Empty;
        public DateTime? FechaHoraModificado { get; protected set; }

        /// <summary>
        /// Valida y establece el usuario que crea la entidad.
        /// </summary>
        protected void EstablecerCreador(string usuario)
        {
            CreadoPor = ValidarUsuario(usuario, nameof(usuario));
            FechaHoraCreado = DateTime.UtcNow;
        }

        /// <summary>
        /// Actualiza la auditoría con el usuario que modifica.
        /// </summary>
        public void ActualizarAuditoria(string usuario)
        {
            ModificadoPor = ValidarUsuario(usuario, nameof(usuario));
            FechaHoraModificado = DateTime.UtcNow;
        }

        /// <summary>
        /// Valida el nombre de usuario según las reglas de negocio.
        /// </summary>
        /// <param name="usuario">Nombre del usuario.</param>
        /// <param name="paramName">Nombre del parámetro para la excepción.</param>
        /// <returns>Usuario validado y trimmed.</returns>
        /// <exception cref="ArgumentException">Si el usuario es nulo o vacío.</exception>
        /// <exception cref="DomainException">Si el usuario excede la longitud máxima.</exception>
        protected static string ValidarUsuario(string usuario, string paramName)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El nombre de usuario es obligatorio.", paramName);

            if (usuario.Length > 100)
                throw new DomainException("El nombre de usuario no puede exceder 100 caracteres.");

            return usuario.Trim();
        }
    }
}