using PetMarketRfullApi.Data.Contexts;

namespace PetMarketRfullApi.Data.Repositories
{
    public abstract class BaseRepositories
    {
        //Контекст БД

        protected readonly AppDbContext _appDbContext;

        public BaseRepositories(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


    }
}
