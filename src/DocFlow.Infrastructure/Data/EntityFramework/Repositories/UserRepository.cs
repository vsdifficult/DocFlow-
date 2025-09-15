
using DocFlow.Application.Repositories;
using DocFlow.Domain.Dtos;
using DocFlow.Domain.Entities;
using DocFlow.Domain.Models;
using DocFlow.Infrastructure.Data.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DocFlowDbContext _context;

        public UserRepository(DocFlowDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(UserDto entity)
        {
            var userEntity = UserMapper.ToEntity(entity);
            userEntity = userEntity with { Id = Guid.NewGuid() };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => UserMapper.ToDto(u))
                .ToListAsync();
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user == null ? null : UserMapper.ToDto(user);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : UserMapper.ToDto(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user == null ? null : UserMapper.ToDto(user);
        }

        public Task<List<UserDto>> GetUnverifiedOlderThanAsync(DateTime cutoff)
        {
            // INFO: Cannot implement this method because there is no timestamp on the UserEntity.
            // A CreatedAt property on UserEntity would be required.
            return Task.FromResult(new List<UserDto>());
        }

        public async Task<UserRole> GetUserRoleAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Role ?? UserRole.Guest;
        }

        public async Task<bool> IsUserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .Select(u => UserMapper.ToDto(u))
                .ToListAsync();
        }

        public async Task<bool> SetEmailVerifiedAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            var updatedUser = user with { EmailVerified = true };
            
            _context.Users.Remove(user);
            await _context.Users.AddAsync(updatedUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SetVerificationCodeAsync(string email, string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            
            var updatedUser = user with { Code = code };

            _context.Users.Remove(user);
            await _context.Users.AddAsync(updatedUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> UpdateAsync(UserDto entity)
        {
            var user = await _context.Users.FindAsync(entity.Id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            var updatedUser = UserMapper.ToEntity(entity);
            await _context.Users.AddAsync(updatedUser);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePasswordAsync(Guid userid, string new_password)
        {
            var user = await _context.Users.FindAsync(userid);
            if (user == null)
            {
                return false;
            }

            var updatedUser = user with { Password = new_password };

            _context.Users.Remove(user);
            await _context.Users.AddAsync(updatedUser);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
