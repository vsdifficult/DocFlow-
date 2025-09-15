using DocFlow.Domain.Models; 

namespace DocFlow.Domain.Entities;

public class UserEntity
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required UserRole Role { get; init; } 

}