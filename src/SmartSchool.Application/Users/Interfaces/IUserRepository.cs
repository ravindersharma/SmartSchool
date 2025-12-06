using SmartSchool.Domain.Entities;
using SmartSchool.Shared;

namespace SmartSchool.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<User?> GetByIdWithRefreshTokenAsunc(Guid id, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken ct);
        Task<User?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken ct);
        Task<PagedResult<User>> GetPagedAsync(int page, int pageSzie, CancellationToken ct);
        Task<User> AddAsync(User user, CancellationToken ct);
        Task UpdateAsync(User user, CancellationToken ct);
        Task DeleteAsync(User user, CancellationToken ct);
    }
}
