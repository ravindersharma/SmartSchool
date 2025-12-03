using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;
using System.Runtime.Serialization;

namespace SmartSchool.Infrastructure.Repositories
{
    public class UserRepository : IUserRspository
    {
        private readonly SchoolDbContext _db;

        public UserRepository(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddAsync(User user, CancellationToken ct)
        {
            await _db.Set<User>().AddAsync(user);
            await _db.SaveChangesAsync(ct);
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await _db.Set<User>().Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByIdAsunc(Guid id, CancellationToken ct)
        {
            return await _db.Set<User>().Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            _db.Set<User>().Update(user);
            await _db.SaveChangesAsync(ct);
        }
    }
}
