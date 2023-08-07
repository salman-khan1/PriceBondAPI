using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.DrawRepository
{
    public interface IDrawRepository
    {
        Task<List<Draw>> GetAllAsync();
        Task<Draw?> GetByIdAsync(int id);
        Task<Draw> CreateAsync(Draw draw);
        Task<Draw?> UpdateAsync(int id, Draw draw);
        Task<Draw?> DeleteAsync(int id);

    }
}
