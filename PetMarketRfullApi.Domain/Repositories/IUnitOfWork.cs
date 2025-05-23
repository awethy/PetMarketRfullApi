﻿namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IPetRepository Pets { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }

        Task<int> SaveChangesAsync();
    }
}
