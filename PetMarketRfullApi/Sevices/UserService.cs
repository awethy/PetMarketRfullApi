using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Sevices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResource> CreateUserAsync(CreateUserResource createUserResource)
        {
            var existingUser = await _unitOfWork.Users.GetByNameAsync(createUserResource.Name);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with the same name|email already exists.");
            }

            var user = _mapper.Map<User>(createUserResource);
            var createdUser = await _unitOfWork.Users.AddUserAsync(user);
            return _mapper.Map<UserResource>(createdUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            var existingUser = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("Not found user.");
            }
            await _unitOfWork.Users.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserResource>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResource>>(users);
        }

        public async Task<UserResource> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("Not found user.");
            }
            return _mapper.Map<UserResource>(user);
        }

        public async Task UpdateUserAsync(int id, UpdateUserResource updateUserResource)
        {
            var existingUser = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            //проверяем, существует ли User с таким же именем (кроме текущей)
            var userWithSameName = await _unitOfWork.Users.GetByNameAsync(updateUserResource.Name);
            if (userWithSameName != null && userWithSameName.Id != id)
            {
                throw new InvalidOperationException("User with the same name already exists.");
            }

            existingUser.Name = updateUserResource.Name;
            existingUser.Email = updateUserResource.Email;
            existingUser.Password = updateUserResource.Password;
            //existingUser.Role = updateUserResource.Role;

            var user = _mapper.Map<User>(existingUser);

            await _unitOfWork.Users.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
