
namespace SmartSchool.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(Guid userId, string email, string role, out DateTime expiresAt);
        string GenerateRefreshToken();
    }
}