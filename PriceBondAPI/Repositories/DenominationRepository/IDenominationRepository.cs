using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.DenominationRepository
{
    public interface IDenominationRepository
    {
        Task<List<Denomination>> GetAllAsync();
        Task<Denomination?> GetByIdAsync(int id);
        Task<Denomination> CreateAsync(Denomination denomination);
        Task<Denomination?> UpdateAsync(int id, Denomination denomination);
        Task<Denomination?> DeleteAsync(int id);

    }
}
