using FluentResults;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Auth.Interfaces
{
    public interface IRefreshTokenRespository
    {
        Task<RefreshToken> AddAsync(RefreshToken refreshToken, CancellationToken ct);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
        Task RevokeAsync(RefreshToken refreshToken, CancellationToken ct);
    }
}
