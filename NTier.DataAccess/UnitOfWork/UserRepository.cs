using Microsoft.EntityFrameworkCore;
using NTier.DataAccess.Data;
using NTier.DataAccess.Repositories;
using NTier.DataObject.Entities;

namespace NTier.DataAccess.UnitOfWork
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
