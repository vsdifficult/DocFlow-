using DocFlow.Domain.Dtos;
using DocFlow.Domain.Entities;

namespace DocFlow.Infrastructure.Data.EntityFramework.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(UserEntity entity)
        {
            return new UserDto 
            {
                Id = entity.Id,
                Email = entity.Email,
                Username = entity.Username,
                Role = entity.Role,
                Password = entity.Password,
                Code = entity.Code,
                EmailVerified = entity.EmailVerified
            };
        }

        public static UserEntity ToEntity(UserDto dto)
        {
            return new UserEntity
            {
                Id = dto.Id,
                Email = dto.Email,
                Username = dto.Username,
                Role = dto.Role,
                Password = dto.Password,
                Code = dto.Code,
                EmailVerified = dto.EmailVerified
            };
        }
    }
}