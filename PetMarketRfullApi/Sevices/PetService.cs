using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.PetsResources;

namespace PetMarketRfullApi.Sevices
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PetResource> CreatePetAsync(CreatePetResource createPetResource)
        {
            var existingPet = await _unitOfWork.Pets.GetByNameAsync(createPetResource.Name);
            if (existingPet != null)
            {
                throw new InvalidOperationException("Pet with the same name already exists.");
            }

            var pet = _mapper.Map<Pet>(createPetResource);
            var createdPet = await _unitOfWork.Pets.AddPetAsync(pet);
            return _mapper.Map<PetResource>(createdPet);
        }

        public async Task DeletePetAsync(int id)
        {
            var existingPet = await _unitOfWork.Pets.GetPetByIdAsync(id);
            if (existingPet == null)
            {
                throw new InvalidOperationException("Not found pet");
            }
            await _unitOfWork.Pets.DeletePetAsync(id);
        }

        public async Task<IEnumerable<PetResource>> GetAllPetsAsync()
        {
            var pets = await _unitOfWork.Pets.GetAllPetsAsync();
            return _mapper.Map<IEnumerable<PetResource>>(pets);
        }

        public async Task<PetResource> GetPetByIdAsync(int id)
        {
            var pet = await _unitOfWork.Pets.GetPetByIdAsync(id);
            if (pet == null)
            {
                throw new InvalidOperationException("Not found pet");
            }
            return _mapper.Map<PetResource>(pet);
        }

        public async Task UpdatePetAsync(int id, UpdatePetResource updatePetResource)
        {
            var existingPet = await _unitOfWork.Pets.GetPetByIdAsync(id);
            if (existingPet == null)
            {
                throw new KeyNotFoundException("Pet not found.");
            }

            //проверяем, существует ли Pet с таким же именем (кроме текущей)
            var petWithSameName = await _unitOfWork.Pets.GetByNameAsync(updatePetResource.Name);
            if (petWithSameName != null && petWithSameName.Id != id)
            {
                throw new InvalidOperationException("Pet with the same name already exists.");
            }

            existingPet.Name = updatePetResource.Name;
            existingPet.Description = updatePetResource.Description;
            existingPet.DateOfBirth = updatePetResource.DateOfBirth;
            existingPet.Price = updatePetResource.Price;
            existingPet.IsAvailable = updatePetResource.IsAvailable;
            existingPet.CategoryId = updatePetResource.CategoryId;

            var pet = _mapper.Map<Pet>(existingPet);

            await _unitOfWork.Pets.UpdatePetAsync(pet);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
