using System;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IHttpContextAccessor httpContextAccessor, IBudgetService _budgetService) : IUserService
{

    public async Task<AppUser?> GetUserProfileAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public bool IsUserAuthenticated()
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.Identity?.IsAuthenticated == true;
    }

    public async Task<SignInResult> LoginUserAsync(LoginDto loginDto)
    {
        var signInResult = await signInManager.PasswordSignInAsync(
            loginDto.Email, loginDto.Password, isPersistent: true, lockoutOnFailure: true);
        if (signInResult.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                _budgetService.CheckBudgetStatus(user.Id);
            }
        }

        return signInResult;
    }

    public async Task LogOutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName
        };

        return await userManager.CreateAsync(user, registerDto.Password);
    }

    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await Task.FromResult(userManager.Users.ToList());
    }

    public async Task<AppUser?> GetUserByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }


    public async Task<AppUser?> GetUserByEmailAsync(string userEmail)
    {
        return await userManager.FindByEmailAsync(userEmail);
    }

    public async Task<AppUser?> UpdateUserAsync(string userId, AppUser updatedUser)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.UserName = updatedUser.UserName;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return null; 

        return user;
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User Id : "+userId+" not found");
        return await userManager.DeleteAsync(user);
    }
}
