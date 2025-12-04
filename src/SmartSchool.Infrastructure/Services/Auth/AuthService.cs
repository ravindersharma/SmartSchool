using FluentResults;
using Microsoft.Extensions.Configuration;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Application.Common.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Services.Email;
using System.Security.Cryptography;

namespace SmartSchool.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRspository _userRepo;
        private readonly IRefreshTokenRespository _refreshTokenRepo;
        private readonly IPasswordResetTokenRespository _passwordResetTokenRespository;
        private readonly IJwtService _jwt;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueue _emailQueue;
        public AuthService(IUserRspository userRepo, IRefreshTokenRespository refreshTokenRepo, IJwtService jwt, IConfiguration config, IPasswordResetTokenRespository passwordResetTokenRespository, IEmailSender
         emailSender, IEmailTemplateService emailTemplateService, IEmailQueue emailQueue)
        {
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _jwt = jwt;
            _config = config;
            _passwordResetTokenRespository = passwordResetTokenRespository;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
            _emailQueue = emailQueue;
        }

        public async Task<Result> ForgotPasswordAsync(string email, string origin, CancellationToken ct)
        {
            var user = await _userRepo.GetByEmailAsync(email.ToLowerInvariant(), ct);
            if (user == null) return Result.Fail("User not found");

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var reset = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(3),
                CreatedAt = DateTime.UtcNow,
                Used = false
            };

            await _passwordResetTokenRespository.AddAsync(reset, ct);

            // Send email with origin + token link (not implemented here) - return token for dev
            var template = _emailTemplateService.Render("password-reset",
                        new Dictionary<string, string>
                        {
                            { "UserName", user.UserName },
                            { "ResetUrl", $"{origin}/reset-password?token={reset.Token}" }
                        }
                    );

            await _emailSender.SendAsync(user.Email, "Password Reset", template);

            return Result.Ok();
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto dto, string ipAddress, CancellationToken ct)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email.ToLowerInvariant(), ct);
            if (user == null) return Result.Fail<AuthResponseDto>("Invalid email or password");

            var ok = PasswordHasher.Verify(dto.Password, user.PasswordHash);
            if (!ok) return Result.Fail<AuthResponseDto>("Invalid email or password");

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user, ct);

            //Create JWT token
            var jwtToken = _jwt.GenerateJwtToken(user.Id, user.Email, user.Role, out var jwtExpiresAt);
            var refereshToken = _jwt.GenerateRefreshToken();
            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refereshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(int.TryParse(_config["Jwt:RefreshTokenDays"], out var d) ? d : 7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIP = ipAddress,
                UserId = user.Id,
            };

            await _refreshTokenRepo.AddAsync(refresh, ct);
            var dtoOut = new AuthResponseDto(user.Id, user.Email, user.UserName, user.Role, jwtToken, refereshToken, jwtExpiresAt);
            return Result.Ok(dtoOut);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string token, string ipAddress, CancellationToken ct)
        {
            var refreshToken = await _refreshTokenRepo.GetByTokenAsync(token, ct);
            if (refreshToken == null || !refreshToken.IsActive) return Result.Fail<AuthResponseDto>("Invalid refresh token");

            //Rotate token
            refreshToken.Revoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIP = ipAddress;

            //Update in db on revoke
            await _refreshTokenRepo.RevokeAsync(refreshToken, ct);

            var newRefreshToken = _jwt.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(int.TryParse(_config["Jwt:RefreshTokenDays"], out var d) ? d : 7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIP = ipAddress,
                UserId = refreshToken.UserId,
            };

            refreshToken.ReplacedByToken = newRefreshToken;
            await _refreshTokenRepo.AddAsync(refreshToken, ct);

            var jwtToken = _jwt.GenerateJwtToken(refreshToken.User!.Id, refreshToken.User.Email, refreshToken.User.Role, out var jwtExpiresAt);
            var dtoOut = new AuthResponseDto(refreshToken.User.Id, refreshToken.User.Email, refreshToken.User.UserName, refreshToken.User.Role, jwtToken, newRefreshToken, jwtExpiresAt);

            return Result.Ok(dtoOut);
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto, string origin, CancellationToken ct)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email, ct);
            if (existing != null) return Result.Fail<AuthResponseDto>("Email already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.ToLowerInvariant(),
                UserName = dto.UserName,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };


            await _userRepo.AddAsync(user, ct);
            // Optional: send confirmation email using 'origin' and a token
            // For now we auto-confirm (or keep false per your policies)
            user.IsEmailConfirmed = true;
            await _userRepo.UpdateAsync(user, ct);

            //Create JWT token
            var jwtToken = _jwt.GenerateJwtToken(user.Id, user.Email, user.Role, out var jwtExpiresAt);
            var refereshToken = _jwt.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refereshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(int.TryParse(_config["Jwt:RefreshTokenDays"], out var d) ? d : 7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIP = "0.0.0.0",
                UserId = user.Id,
            };


            await _refreshTokenRepo.AddAsync(refresh, ct);

            var dtoOut = new AuthResponseDto(user.Id, user.Email, user.UserName, user.Role, jwtToken, refereshToken, jwtExpiresAt);

            // Send welcome email using Worker queue
            var body = _emailTemplateService.Render("welcome", new()
            {
                { "UserName", dtoOut.UserName }
            });

            await _emailQueue.EnqueueAsync(new QueuedEmail(
                dtoOut.Email,
                "Welcome to SmartSchool",
                body
            ));

            return Result.Ok(dtoOut);
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto dto, CancellationToken ct)
        {
            var reset = await _passwordResetTokenRespository.GetByTokenAsync(dto.Token, ct);
            if (reset == null || reset.Used || reset.ExpiresAt < DateTime.UtcNow) return Result.Fail("Invalid or expired token");

            var user = await _userRepo.GetByIdAsunc(reset.UserId, ct);
            if (user == null) return Result.Fail("User not found");

            user.PasswordHash = PasswordHasher.Hash(dto.NewPassword);
            reset.Used = true;
            await _passwordResetTokenRespository.UpdateAsync(reset, ct);
            return Result.Ok();
        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string ipAddress, CancellationToken ct)
        {
            var refreshToken = await _refreshTokenRepo.GetByTokenAsync(token, ct);
            if (refreshToken == null || !refreshToken.IsActive) return Result.Fail("Token not found or inactive");

            //Rotate token
            refreshToken.Revoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIP = ipAddress;

            //Update in db on revoke
            await _refreshTokenRepo.RevokeAsync(refreshToken, ct);

            return Result.Ok();
        }
    }
}
