using DocFlow.Domain.Dtos;
using DocFlow.Domain.Models;

namespace DocFlow.Application.Repositories;

/// <summary>
/// Repository for user-related operations
/// </summary>
public interface IUserRepository : IRepository<UserDto, Guid>
{
    Task<bool> UpdatePasswordAsync(Guid userid, string new_password);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<List<UserDto>> GetUnverifiedOlderThanAsync(DateTime cutoff);
    Task<UserRole> GetUserRoleAsync(Guid userId);
    Task<bool> IsUserExistsByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role);

    // Email verification methods
    Task<bool> SetEmailVerifiedAsync(string email);
    Task<bool> SetVerificationCodeAsync(string email, string code);
    Task<int> GetUserCountAsync();
}