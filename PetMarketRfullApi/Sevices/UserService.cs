using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;

namespace PetMarketRfullApi.Sevices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
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

        public async Task DeleteUserAsync(string id)
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

        public async Task<UserResource> GetUserByIdAsync(string id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("Not found user.");
            }
            return _mapper.Map<UserResource>(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UpdateUserResource updateUserResource)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            //проверяем, существует ли User с таким же именем (кроме текущей)
            //var userWithSameName = await _unitOfWork.Users.GetByNameAsync(updateUserResource.Name);
            //if (userWithSameName != null && userWithSameName.id != id)
            //{
            //    throw new InvalidOperationException("User with the same name already exists.");
            //}

            user.UserName = updateUserResource.Name;
            user.Email = updateUserResource.Email;
            user.PhoneNumber = updateUserResource.PhoneNumber;

            var mUser = _mapper.Map<User>(user);

            return await _userManager.UpdateAsync(mUser);
        }
    }
}
