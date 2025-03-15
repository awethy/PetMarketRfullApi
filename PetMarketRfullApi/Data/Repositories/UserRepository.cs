using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class UserRepository : BaseRepositories, IUserRepository
    {
        // Конструктор для внедрения зависимости через DI
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<User> AddUserAsync(User user)
        {
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _appDbContext.Users.FindAsync(id);
            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _appDbContext.Users
                .ToListAsync();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _appDbContext.Users
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _appDbContext.Users
                .FindAsync(id);
                //.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _appDbContext.Update(user);
        }
    }
}
