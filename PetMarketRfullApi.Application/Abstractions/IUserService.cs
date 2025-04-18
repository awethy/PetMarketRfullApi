﻿using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Application.Resources.UsersResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IUserService
    {
        // Получить все user
        Task<IEnumerable<UserResource>> GetAllUsersAsync();

        // Получить user по id
        Task<UserResource> GetUserByIdAsync(string id);

        // Добавить новую user
        //Task<UserResource> CreateUserAsync(CreateUserResource createUserResource);

        // Обновить существующую user
        Task<IdentityResult> UpdateUserAsync(string id, UpdateUserResource updateUserResource);

        // Удалить user по id
        Task DeleteUserAsync(string id);
    }
}
