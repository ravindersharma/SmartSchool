using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Auth.Interfaces
{
    public interface IUserRspository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByIdAsunc(Guid id, CancellationToken ct);
        Task<User> AddAsync(User user, CancellationToken ct);
        Task UpdateAsync(User user, CancellationToken ct);
    }
}
