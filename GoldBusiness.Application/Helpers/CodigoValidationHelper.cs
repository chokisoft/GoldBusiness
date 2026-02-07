using Microsoft.Extensions.Localization;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Helpers
{
    /// <summary>
    /// Helper genérico para validar códigos duplicados en cualquier entidad.
    /// </summary>
    public static class CodigoValidationHelper
    {
        /// <summary>
        /// Valida si un código está duplicado para una nueva entidad.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad</typeparam>
        /// <param name="repository">Repositorio de la entidad</param>
        /// <param name="codigo">Código a validar</param>
        /// <param name="localizer">Localizador de mensajes</param>
        /// <returns>Tupla: (existe, estaCancelado, entidadExistente)</returns>
        public static async Task<(bool existe, bool estaCancelado, T? entidad)> ValidateCodigoForCreateAsync<T>(
            IBaseRepositoryWithCode<T> repository,
            string codigo) where T : class
        {
            var existingEntity = await repository.GetByCodigoAsync(codigo, includeCanceled: true);

            if (existingEntity == null)
                return (false, false, null);

            // Verificar si está cancelado usando reflexión
            var canceladoProperty = typeof(T).GetProperty("Cancelado");
            if (canceladoProperty != null)
            {
                var estaCancelado = (bool)(canceladoProperty.GetValue(existingEntity) ?? false);
                return (true, estaCancelado, existingEntity);
            }

            return (true, false, existingEntity);
        }

        /// <summary>
        /// Valida si un código está duplicado para actualizar una entidad.
        /// </summary>
        public static async Task<(bool existe, bool estaCancelado, int? existingId)> ValidateCodigoForUpdateAsync<T>(
            IBaseRepositoryWithCode<T> repository,
            string codigo,
            int currentId) where T : class
        {
            var existingEntity = await repository.GetByCodigoAsync(codigo, includeCanceled: true);

            if (existingEntity == null)
                return (false, false, null);

            // Obtener el ID de la entidad existente
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var existingId = (int)(idProperty.GetValue(existingEntity) ?? 0);

                if (existingId == currentId)
                    return (false, false, null); // Es la misma entidad

                // Verificar si está cancelado
                var canceladoProperty = typeof(T).GetProperty("Cancelado");
                if (canceladoProperty != null)
                {
                    var estaCancelado = (bool)(canceladoProperty.GetValue(existingEntity) ?? false);
                    return (true, estaCancelado, existingId);
                }

                return (true, false, existingId);
            }

            return (false, false, null);
        }

        /// <summary>
        /// Crea el mensaje de error apropiado para código duplicado.
        /// </summary>
        public static string GetDuplicateCodeErrorMessage(
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer,
            string codigo,
            bool estaCancelado,
            int? existingId = null)
        {
            if (estaCancelado && existingId.HasValue)
            {
                return string.Format(localizer["RegistroCanceladoExistente"].Value, codigo, existingId.Value);
            }

            return string.Format(localizer["CodigoDuplicado"].Value, codigo);
        }
    }
}