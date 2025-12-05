using SmartSchool.Domain.Enums;

namespace SmartSchool.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public Role Role { get; set; } = Role.Student;
        public bool IsEmailConfirmed { get; set; } = false;
        public DateTime LastLoginAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
