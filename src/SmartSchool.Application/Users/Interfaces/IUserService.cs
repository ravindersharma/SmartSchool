using FluentResults;
using SmartSchool.Application.Users.Dtos;
using SmartSchool.Shared;

namespace SmartSchool.Application.Users.Interfaces
{
    public interface IUserService
    {
        // Called by AuthService during register
        Task<Result<UserDto>> CreateFromAuthAsync(CreateUserDto dto, CancellationToken ct);
        // Admin create or system create
        Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken ct);
        Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct);
        Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct);
        Task<Result> RestoreAsync(Guid id, CancellationToken ct);
        Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Result<PagedResult<UserDto>>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<Result> AssignRoleAsync(Guid id, string role, CancellationToken ct);
        Task<Result> DeleteAsync(Guid id, CancellationToken ct);
    }
}
