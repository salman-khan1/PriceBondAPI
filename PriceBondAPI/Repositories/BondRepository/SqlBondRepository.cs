using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.BondRepository
{
    public class SqlBondRepository : IBondRepository
    {
        private readonly PbdatabaseContext _context;

        public SqlBondRepository(PbdatabaseContext context)
        {
            _context = context;
        }
        public async Task<Bond> CreateAsync(Bond bond)
        {
            await _context.Bonds.AddAsync(bond);
            await _context.SaveChangesAsync();
            return bond;
        }

        public async Task<Bond?> DeleteAsync(int id)
        {
            var existingBond=await _context.Bonds.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBond == null) { return null; }
            _context.Bonds.Remove(existingBond);
            await _context.SaveChangesAsync();
            return existingBond;
        }

        public async Task<List<Bond>> GetAllAsync()
        {
            return await _context.Bonds.ToListAsync();
        }

        public async Task<Bond?> GetByIdAsync(int id)
        {
           return await _context.Bonds.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Bond?> UpdateAsync(int id, Bond bond)
        {
            var existingBond = await _context.Bonds.FirstOrDefaultAsync(x => x.Id == id);
            if (existingBond == null)
            {
                return null;
            }
            existingBond.BondNumber = bond.BondNumber;
            existingBond.PurchaseDate= bond.PurchaseDate;
            existingBond.UserId = bond.UserId;
            existingBond.DenominationId = bond.DenominationId;

            await _context.SaveChangesAsync();
            return existingBond;
        }
    }
}
