namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IPetRepository Pets { get; }
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
    }
}
