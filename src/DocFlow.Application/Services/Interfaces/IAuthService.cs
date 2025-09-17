using DocFlow.Domain.Dtos;
using DocFlow.Domain.Models;

namespace DocFlow.Application.Services.Interfaces;

public record AuthResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public string? Message { get; init; }
    public Guid? UserId { get; init; }
    public string? Token { get; init; }
    public int Code { get; init; }
    public UserRole? Role { get; init; }
}


/// <summary>
/// Service for authentication operations
/// </summary>
public interface IAuthenticationService
{

    Task<AuthResult> SendCodeAgain(string email);
    Task<AuthResult> DeleteUserAsync(Guid userId);
    Task<AuthResult> SignInAsync(UserLoginDto loginDto);
    Task<AuthResult> SignUpAsync(UserRegistrationDto registrationDto);
    Task<bool> SignOutAsync(string token);
    Task<UserRole> GetUserRoleAsync(Guid userId);
    Task<Guid?> GetUserIdFromTokenAsync(string token);
    Task<AuthResult> VerifyEmailAsync(EmailVerificationDto verificationDto);
    Task<AuthResult> RequestPasswordResetAsync(PasswordResetRequestDto dto);
    Task<AuthResult> ConfirmPasswordResetAsync(PasswordResetConfirmDto dto);
}