using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Users.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Shared;
using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolDbContext _db;

        public UserRepository(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddAsync(User user, CancellationToken ct)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant() && !u.IsDeleted, ct);
        }
        public async Task<User?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken ct)
        {
            return await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant() && !u.IsDeleted, ct);
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);
        }
        public async Task<User?> GetByIdWithRefreshTokenAsunc(Guid id, CancellationToken ct)
        {
            return await _db.Set<User>().Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);
        }
        public Task<User?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken ct)
        {
            return _db.Users.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id, ct);
        }
        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(User user, CancellationToken ct)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
        {
            var query = _db.Users.AsNoTracking().Where(u => !u.IsDeleted).OrderBy(u => u.CreatedAt);
            var totalItems = await query.CountAsync(ct);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return new PagedResult<User>(items, totalItems, page, pageSize);
        }

    }
}
