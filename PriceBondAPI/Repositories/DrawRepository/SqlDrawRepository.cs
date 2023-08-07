using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.DrawRepository
{
    public class SqlDrawRepository : IDrawRepository
    {
        private readonly PbdatabaseContext _context;

        public SqlDrawRepository(PbdatabaseContext context)
        {
            _context = context;
        }
        public async Task<Draw> CreateAsync(Draw draw)
        {
            await _context.Draws.AddAsync(draw); 
            await _context.SaveChangesAsync();
            return draw;
        }

        public async Task<Draw?> DeleteAsync(int id)
        {
            var existingDraw= await _context.Draws.FirstOrDefaultAsync(draw => draw.Id == id);
            if (existingDraw == null) { return null; }
            _context.Draws.Remove(existingDraw);
            await _context.SaveChangesAsync();
            return existingDraw;
        }

        public async Task<List<Draw>> GetAllAsync()
        {
           return await _context.Draws.ToListAsync();
  
        }

        public async Task<Draw?> GetByIdAsync(int id)
        {
           return await _context.Draws.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Draw?> UpdateAsync(int id, Draw draw)
        {
            var existingDraw = await _context.Draws.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDraw == null)
            {
                return null;
            }
            existingDraw.DrawDate = draw.DrawDate;
            existingDraw.DrawLocation= draw.DrawLocation;
            existingDraw.Price= draw.Price;
            existingDraw.DenominationId= draw.DenominationId;
            await _context.SaveChangesAsync();
            return existingDraw;
        }
    }
}
