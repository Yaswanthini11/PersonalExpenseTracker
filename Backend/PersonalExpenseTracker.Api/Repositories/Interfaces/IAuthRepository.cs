using Microsoft.AspNetCore.Identity;
using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    Task<ApplicationUser?> GetUserByIdAsync(string userId);

    Task<IdentityResult> CreateUserAsync(
        ApplicationUser user,
        string password);

    Task<bool> CheckPasswordAsync(
        ApplicationUser user,
        string password);
}