using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Auth.Interfaces
{
    public interface IPasswordResetTokenRespository
    {
        Task<PasswordResetToken> AddAsync(PasswordResetToken passwordResetToken, CancellationToken ct);
        Task<PasswordResetToken> UpdateAsync(PasswordResetToken passwordResetToken, CancellationToken ct);
        Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken ct);
    }
}
