namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IPetRepository Pets { get; }

        Task<int> SaveChangesAsync();
    }
}
