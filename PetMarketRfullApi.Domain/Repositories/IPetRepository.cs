using PetMarketRfullApi.Domain.Models.Products;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IPetRepository
    {
        // Получить все pets
        Task<IEnumerable<Pet>> GetAllPetsAsync();

        // Получить pet по id
        Task<Pet> GetPetByIdAsync(int id);

        // Добавить новую pet
        Task<Pet> AddPetAsync(Pet pet);

        // Обновить существующую pet
        Task UpdatePetAsync(Pet pet);

        // Удалить pet по id
        Task DeletePetAsync(int id);
        Task<Pet> GetByNameAsync(string name);
    }
}
