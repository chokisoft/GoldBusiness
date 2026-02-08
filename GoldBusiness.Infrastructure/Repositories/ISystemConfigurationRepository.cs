using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ISystemConfigurationRepository : IBaseRepositoryWithCode<SystemConfiguration>
    {
        Task<IEnumerable<SystemConfiguration>> GetAllAsync();
        Task<SystemConfiguration?> GetByIdAsync(int id);
        Task AddAsync(SystemConfiguration entity);
        Task UpdateAsync(SystemConfiguration entity);
    }
}