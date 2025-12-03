using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure.Repositories
{
    public class RefreshTokenRespository : IRefreshTokenRespository
    {
        private readonly SchoolDbContext _db;
        public RefreshTokenRespository(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshToken> AddAsync(RefreshToken refreshToken, CancellationToken ct)
        {
             _db.Set<RefreshToken>().Add(refreshToken);
            await _db.SaveChangesAsync(ct);
            return refreshToken;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
        {
            return await _db.Set<RefreshToken>().Include(r => r.User).FirstOrDefaultAsync(rt => rt.Token == token, ct);
        }

        public async Task RevokeAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            _db.Set<RefreshToken>().Update(refreshToken);
            await _db.SaveChangesAsync(ct);
        }
    }
}
