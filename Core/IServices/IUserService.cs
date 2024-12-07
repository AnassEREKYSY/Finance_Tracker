using System;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.IServices;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
    Task<SignInResult> LoginUserAsync(LoginDto loginDto);
    Task<AppUser?> GetUserProfileAsync(string userId);
    bool IsUserAuthenticated();
    Task LogOutAsync();
    Task<List<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(string userId);
    Task<AppUser?> UpdateUserAsync(string userId, AppUser updatedUser);
    Task<IdentityResult> DeleteUserAsync(string userId);
    Task<AppUser?> GetUserByEmailAsync(string userEmail);
}
