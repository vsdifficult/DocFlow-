using DocFlow.Domain.Models;

namespace DocFlow.Domain.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; } 
    public string Password { get; init; } 
    public string Code { get; init; }
    public bool EmailVerified { get; init; } 
    public UserRole Role { get; init; }
}

public record UserLoginDto
{
    public string Email { get; init; }
    public string Password { get; init; } 
}

public record UserRegistrationDto
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string Username { get; init; } 
    public UserRole Role { get; init; }
}

public record EmailVerificationDto
{
    public string Email { get; init; } 
    public string Code { get; init; }
}

public record PasswordResetRequestDto
{
    public string Email { get; init; }
}

public record PasswordResetConfirmDto
{
    public string Email { get; init; }
}
