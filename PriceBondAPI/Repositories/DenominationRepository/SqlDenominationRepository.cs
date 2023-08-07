using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.DenominationRepository
{
    public class SqlDenominationRepository : IDenominationRepository
    {
        private readonly PbdatabaseContext _context;

        public SqlDenominationRepository(PbdatabaseContext context)
        {
            _context = context;
        }

        public async Task<Denomination> CreateAsync(Denomination denomination)
        {
            await _context.Denominations.AddAsync(denomination);
           await _context.SaveChangesAsync();
            return denomination;
        }

        public async Task<Denomination?> DeleteAsync(int id)
        {
            var existingDenomination = await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDenomination == null) { return null; }
             _context.Denominations.Remove(existingDenomination);
            await _context.SaveChangesAsync();
            return existingDenomination;

        }

        public async Task<List<Denomination>> GetAllAsync()
        {
            return await _context.Denominations.ToListAsync();
        }

        public async Task<Denomination?> GetByIdAsync(int id)
        {
            return await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Denomination?> UpdateAsync(int id, Denomination denomination)
        {
            var existingDenomination = await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDenomination == null)
            {
                return null;
            }
            existingDenomination.Value = denomination.Value;
            existingDenomination.Description = denomination.Description;

            await _context.SaveChangesAsync();
            return existingDenomination;
        }
    }
}