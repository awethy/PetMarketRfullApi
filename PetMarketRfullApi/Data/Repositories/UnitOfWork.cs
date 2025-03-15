using Microsoft.EntityFrameworkCore.Query;
using PetMarketRfullApi.Controllers;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class UnitOfWork : BaseRepositories, IUnitOfWork
    {
        public UnitOfWork(AppDbContext appDbContext) : base(appDbContext) 
        {
            Categories = new CategoryRepository(_appDbContext);
            Pets = new PetRepository(_appDbContext);
            Users = new UserRepository(_appDbContext);
        }

        public ICategoryRepository Categories { get; }
        public IPetRepository Pets { get; }
        public IUserRepository Users { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
