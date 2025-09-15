using DocFlow.Domain.Models;

namespace DocFlow.Domain.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; }
    public UserRole Role { get; init; }
}