using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources.PetsResources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IPetService
    {
        // Получить все pets
        Task<IEnumerable<PetResource>> GetAllPetsAsync();

        // Получить pet по id
        Task<PetResource> GetPetByIdAsync(int id);

        // Добавить новую pet
        Task<PetResource> CreatePetAsync(CreatePetResource createPetResource);

        // Обновить существующую pet
        Task UpdatePetAsync(int id, UpdatePetResource updatePetResource);

        // Удалить pet по id
        Task DeletePetAsync(int id);
    }
}
