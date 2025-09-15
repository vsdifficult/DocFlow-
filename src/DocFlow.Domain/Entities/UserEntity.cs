
using DocFlow.Domain.Models;

namespace DocFlow.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
        public string? Code { get; set; }
        public bool EmailVerified { get; set; }
    }
}
