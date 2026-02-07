namespace GoldBusiness.Infrastructure.Repositories
{
    /// <summary>
    /// Interfaz base para repositorios que manejan entidades con código único.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public interface IBaseRepositoryWithCode<T> where T : class
    {
        Task<T?> GetByCodigoAsync(string codigo, bool includeCanceled = false);
        Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true);
    }
}