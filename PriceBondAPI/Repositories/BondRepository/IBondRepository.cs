using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.BondRepository
{
    public interface IBondRepository
    {
        Task<List<Bond>> GetAllAsync();
        Task<Bond?> GetByIdAsync(int id);
        Task<Bond> CreateAsync(Bond bond);
        Task<Bond?> UpdateAsync(int id, Bond bond);
        Task<Bond?> DeleteAsync(int id);

    }
}
