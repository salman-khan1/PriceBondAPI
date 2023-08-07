using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;

namespace PriceBondAPI.Repositories.UserRepository
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly PbdatabaseContext _context;

        public SqlUserRepository(PbdatabaseContext context)
        {
           _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser == null) { return null; }
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();  
            return existingUser;
                
        }
        
        public async Task<List<User>> GetAllAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Name= user.Name;
            existingUser.Email = user.Email;
            existingUser.RegistrationDate = user.RegistrationDate;
            await _context.SaveChangesAsync();
            return existingUser;
        }
    }
}
