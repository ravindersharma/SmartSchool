using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure.Repositories
{
    public class PasswordResetTokenRespository : IPasswordResetTokenRespository
    {
        private readonly SchoolDbContext _db;

        public PasswordResetTokenRespository(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task<PasswordResetToken> AddAsync(PasswordResetToken passwordResetToken, CancellationToken ct)
        {
            _db.Set<PasswordResetToken>().Add(passwordResetToken);
            await _db.SaveChangesAsync(ct);

            return passwordResetToken;
        }
        public async Task<PasswordResetToken> UpdateAsync(PasswordResetToken passwordResetToken, CancellationToken ct)
        {
            _db.Set<PasswordResetToken>().Update(passwordResetToken);
            await _db.SaveChangesAsync(ct);

            return passwordResetToken;
        }

        public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken ct)
        {
            return await _db.Set<PasswordResetToken>().FirstOrDefaultAsync(t => t.Token == token, ct);
        }
    }
}
