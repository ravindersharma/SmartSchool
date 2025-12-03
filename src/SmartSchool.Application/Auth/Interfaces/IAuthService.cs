using FluentResults;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto, string origin, CancellationToken ct);
        Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto dto, string ipAddress, CancellationToken ct);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(string token, string ipAddress, CancellationToken ct);
        Task<Result> RevokeRefreshTokenAsync(string token, string ipAddress, CancellationToken ct);
        Task<Result> ForgotPasswordAsync(string email, string origin, CancellationToken ct);
        Task<Result> ResetPasswordAsync(ResetPasswordRequestDto dto, CancellationToken ct);
    }
}
