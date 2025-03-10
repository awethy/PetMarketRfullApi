using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IPetService
    {
        // Получить все pets
        Task<IEnumerable<Pet>> GetAllPetsAsync();

        // Получить pet по id
        Task<PetResource> GetPetByIdAsync(int id);

        // Добавить новую pet
        Task<PetResource> CreatePetAsync();

        // Обновить существующую pet
        Task UpdatePetAsync(int id);

        // Удалить pet по id
        Task DeletePetAsync(int id);
    }
}
