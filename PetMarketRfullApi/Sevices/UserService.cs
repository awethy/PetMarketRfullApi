using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //public async Task<UserResource> CreateUserAsync(CreateUserResource createUserResource)
        //{
        //    var existingUser = await _unitOfWork.Users.GetByNameAsync(createUserResource.Name);
        //    if (existingUser != null)
        //    {
        //        throw new InvalidOperationException("User with the same name|email already exists.");
        //    }

        //    var user = _mapper.Map<User>(createUserResource);
        //    var createdUser = await _unitOfWork.Users.AddUserAsync(user);
        //    return _mapper.Map<UserResource>(createdUser);
        //}

        public async Task<IdentityResult> RegisterUserAsync(CreateUserResource createUserResource)
        {
            try
            {
                var user = _mapper.Map<User>(createUserResource) /*new User { UserName = createUserResource.Name, Email = createUserResource.Email }*/;
                return await _userManager.CreateAsync(user, createUserResource.Password);
            }
            catch (Exception ex) { Console.WriteLine(ex); throw; }
        }

        public async Task<SignInResult> LoginAsync(LoginUserResource userResource)
        {
            var user = await _userManager.FindByEmailAsync(userResource.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return SignInResult.NotAllowed;
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            return await _signInManager.PasswordSignInAsync(user, userResource.Password, userResource.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
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

        public async Task UpdateUserAsync(string id, UpdateUserResource updateUserResource)
        {
            var existingUser = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            //проверяем, существует ли User с таким же именем (кроме текущей)
            var userWithSameName = await _unitOfWork.Users.GetByNameAsync(updateUserResource.Name);
            if (userWithSameName != null && userWithSameName.id != id)
            {
                throw new InvalidOperationException("User with the same name already exists.");
            }

            existingUser.UserName = updateUserResource.Name;
            existingUser.Email = updateUserResource.Email;
            existingUser.PasswordHash = updateUserResource.Password;
            //existingUser.Role = updateUserResource.Role;

            var user = _mapper.Map<User>(existingUser);

            await _unitOfWork.Users.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
