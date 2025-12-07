using FluentResults;
using Mapster;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Application.Users.Dtos;
using SmartSchool.Application.Users.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;
using SmartSchool.Shared;

namespace SmartSchool.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordHasher _hasher;

        public UserService(IUserRepository repo, IPasswordHasher hasher)
        {
            _repo = repo;
            _hasher = hasher;
        }
        public async Task<Result> AssignRoleAsync(Guid id, string role, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null) return Result.Fail("User not found.");

            user.Role = Enum.Parse<Role>(role, true);
            await _repo.UpdateAsync(user, ct);

            return Result.Ok();
        }

        public Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken ct)
        {
            // behavior same for now; could add admin-only differences
            return CreateFromAuthAsync(dto, ct);
        }

        public async Task<Result<UserDto>> CreateFromAuthAsync(CreateUserDto dto, CancellationToken ct)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email, ct);
            if (existing != null)
            {
                return Result.Fail<UserDto>("User with the same email already exists.");
            }

            var user = dto.Adapt<User>();
            user.PasswordHash = _hasher.Hash(dto.Password);

            await _repo.AddAsync(user, ct);

            return Result.Ok(user.Adapt<UserDto>());
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
        {
            var user = await _repo.GetByIdIncludingDeletedAsync(id, ct);
            if (user == null) return Result.Fail("User not found");
            if (!user.IsDeleted) return Result.Fail("User must be soft-deleted before permanent deletion.");

            await _repo.DeleteAsync(user, ct);

            return Result.Ok();

        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null) return Result.Fail<UserDto>("User not found.");

            return Result.Ok(user.Adapt<UserDto>());
        }

        public async Task<Result<PagedResult<UserDto>>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
        {
            var pagedUsers = await _repo.GetPagedAsync(page, pageSize, ct);
            var dtoItems = pagedUsers.Items.Select(u => u.Adapt<UserDto>());
            return Result.Ok(new PagedResult<UserDto>(dtoItems, pagedUsers.TotalCount, page, pageSize));
        }

        public async Task<Result> RestoreAsync(Guid id, CancellationToken ct)
        {
            var user = await _repo.GetByIdIncludingDeletedAsync(id, ct);
            if (user == null) return Result.Fail("User not found");
            if (!user.IsDeleted) return Result.Fail("User is not deleted.");

            user.IsDeleted = false;
            await _repo.UpdateAsync(user, ct);

            return Result.Ok();
        }

        public async Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null) return Result.Fail("User not found.");

            user.IsDeleted = true;
            await _repo.UpdateAsync(user, ct);

            return Result.Ok();
        }

        public async Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null)
            {
                return Result.Fail<UserDto>("User not found.");
            }

            existing.UserName = dto.UserName ?? existing.UserName;
            existing.Role = dto.Role != null ? Enum.Parse<Role>(dto.Role, true) : existing.Role;

            //Updating profile info if student  profile exists  -> Handle in student service

            await _repo.UpdateAsync(existing, ct);

            return Result.Ok(existing.Adapt<UserDto>());

        }
    }
}
