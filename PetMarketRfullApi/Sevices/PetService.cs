using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources;

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

        public Task<PetResource> CreatePetAsync()
        {
            throw new NotImplementedException();
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

        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _unitOfWork.Pets.GetAllPetsAsync();
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

        public async Task UpdatePetAsync(int id)
        {
            
        }
    }
}
