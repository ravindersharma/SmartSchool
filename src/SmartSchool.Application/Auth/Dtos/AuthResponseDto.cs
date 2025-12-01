namespace SmartSchool.Application.Auth.Dtos;

public record AuthResponseDto(Guid UserId, string Email, string UserName, string Role, string JwtToken, string RefreshToken, DateTime JwtExpiresAt);
