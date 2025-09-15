
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
            userEntity.Id = Guid.NewGuid();

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

        public async Task<List<UserDto>> GetUnverifiedOlderThanAsync(DateTime cutoff)
        {
            return await _context.Users
                .Where(u => !u.EmailVerified && u.CreatedAt < cutoff)
                .Select(u => UserMapper.ToDto(u))
                .ToListAsync();
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

            user.EmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SetVerificationCodeAsync(string email, string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            
            user.Code = code;
            user.UpdatedAt = DateTime.UtcNow;

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

            UserMapper.UpdateEntity(user, entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePasswordAsync(Guid userid, string new_password)
        {
            var user = await _context.Users.FindAsync(userid);
            if (user == null)
            {
                return false;
            }
            
            user.Password = new_password;
            user.UpdatedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
