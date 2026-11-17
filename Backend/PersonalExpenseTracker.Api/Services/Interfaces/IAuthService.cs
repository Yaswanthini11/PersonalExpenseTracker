using PersonalExpenseTracker.Api.DTOs.Auth;

namespace PersonalExpenseTracker.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);

    Task<CurrentUserDto?> GetCurrentUserAsync(string userId);
}