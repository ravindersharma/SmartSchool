namespace SmartSchool.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string CreatedByIP { get; set; } = default!;
        public bool Revoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIP { get; set; }
        public string? ReplacedByToken { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public bool IsActive => !Revoked && DateTime.UtcNow < ExpiresAt;
    }
}
