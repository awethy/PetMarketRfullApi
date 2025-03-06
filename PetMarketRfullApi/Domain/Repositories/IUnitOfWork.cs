namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }

        Task<int> SaveChangesAsync();
    }
}
