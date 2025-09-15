using DocFlow.Application.Repositories;
using DocFlow.Application.Services.Interfaces;
using DocFlow.Domain.Dtos;
using DocFlow.Domain.Models;

namespace DocFlow.Infrastructure.Auth;

public class AuthService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResult> SignUpAsync(UserRegistrationDto registrationDto)
    {
        if (await _userRepository.IsUserExistsByEmailAsync(registrationDto.Email))
        {
            return new AuthResult { Success = false, ErrorMessage = "User with this email already exists." };
        }

        // TODO: Use a proper password hashing library
        var hashedPassword = registrationDto.Password;
        var verificationCode = new Random().Next(100000, 999999).ToString();

        var userDto = new UserDto
        {
            Email = registrationDto.Email,
            Username = registrationDto.Username,
            Password = hashedPassword,
            Role = registrationDto.Role,
            Code = verificationCode,
            EmailVerified = false
        };

        var userId = await _userRepository.CreateAsync(userDto);

        // TODO: Send verification code via email

        return new AuthResult { Success = true, UserId = userId };
    }

    public async Task<AuthResult> SignInAsync(UserLoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new AuthResult { Success = false, ErrorMessage = "Invalid email or password." };
        }

        // TODO: Use a proper password hashing library
        if (user.Password != loginDto.Password)
        {
            return new AuthResult { Success = false, ErrorMessage = "Invalid email or password." };
        }

        if (!user.EmailVerified)
        {
            return new AuthResult { Success = false, ErrorMessage = "Email not verified." };
        }

        var token = await GenerateTokenAsync(user.Id, user.Role);

        return new AuthResult { Success = true, Token = token, UserId = user.Id, Role = user.Role };
    }

    public Task<bool> SignOutAsync(string token)
    {
        // INFO: In a real application, you would invalidate the token.
        // This could be done by storing it in a blacklist until it expires.
        return Task.FromResult(true);
    }

    public async Task<AuthResult> VerifyEmailAsync(EmailVerificationDto verificationDto)
    {
        var user = await _userRepository.GetByEmailAsync(verificationDto.Email);
        if (user == null)
        {
            return new AuthResult { Success = false, ErrorMessage = "User not found." };
        }

        if (user.Code != verificationDto.Code)
        {
            return new AuthResult { Success = false, ErrorMessage = "Invalid verification code." };
        }

        await _userRepository.SetEmailVerifiedAsync(verificationDto.Email);

        return new AuthResult { Success = true };
    }

    public async Task<string> GenerateTokenAsync(Guid userId, UserRole role)
    {
        // INFO: In a real application, you would use a proper JWT library to generate a token.
        return await Task.FromResult(Guid.NewGuid().ToString());
    }

    public Task<Guid?> GetUserIdFromTokenAsync(string token)
    {
        // INFO: In a real application, you would use a proper JWT library to validate and parse the token.
        if (Guid.TryParse(token, out var userId))
        {
            return Task.FromResult<Guid?>(userId);
        }
        return Task.FromResult<Guid?>(null);
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        // INFO: In a real application, you would use a proper JWT library to validate the token.
        return Task.FromResult(Guid.TryParse(token, out _));
    }

    public async Task<UserRole> GetUserRoleAsync(Guid userId)
    {
        return await _userRepository.GetUserRoleAsync(userId);
    }

    public async Task<AuthResult> DeleteUserAsync(Guid userId)
    {
        var result = await _userRepository.DeleteAsync(userId);
        return new AuthResult { Success = result };
    }

    public async Task<AuthResult> SendCodeAgain(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return new AuthResult { Success = false, ErrorMessage = "User not found." };
        }

        var verificationCode = new Random().Next(100000, 999999).ToString();
        await _userRepository.SetVerificationCodeAsync(email, verificationCode);

        // TODO: Send verification code via email

        return new AuthResult { Success = true };
    }

    public async Task<AuthResult> RequestPasswordResetAsync(PasswordResetRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return new AuthResult { Success = false, ErrorMessage = "User not found." };
        }

        var resetCode = new Random().Next(100000, 999999).ToString();
        await _userRepository.SetVerificationCodeAsync(dto.Email, resetCode);

        // TODO: Send password reset code via email

        return new AuthResult { Success = true };
    }

    public async Task<AuthResult> ConfirmPasswordResetAsync(PasswordResetConfirmDto dto)
    {
        // This method is not fully implemented as it requires more details on how the password reset is confirmed.
        // For example, it might require a new password to be provided.
        return await Task.FromResult(new AuthResult { Success = false, ErrorMessage = "Not implemented" });
    }
}