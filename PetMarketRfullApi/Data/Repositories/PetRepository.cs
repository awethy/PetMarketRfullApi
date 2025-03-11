using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class PetRepository : BaseRepositories, IPetRepository
    {
        // Конструктор для внедрения зависимости через DI
        public PetRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Pet> AddPetAsync(Pet pet)
        {
            _appDbContext.Pet.Add(pet);
            await _appDbContext.SaveChangesAsync();
            return pet;
        }

        public async Task DeletePetAsync(int id)
        {
            var pet = await _appDbContext.Pet.FindAsync(id);
            if (pet != null)
            {
                _appDbContext.Pet.Remove(pet);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _appDbContext.Pet
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Pet> GetByNameAsync(string name)
        {
            return await _appDbContext.Pet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Pet> GetPetByIdAsync(int id)
        {
            return await _appDbContext.Pet.FindAsync(id);
        }

        public async Task UpdatePetAsync(Pet pet)
        {
            _appDbContext.Pet.Update(pet);
        }
    }
}
