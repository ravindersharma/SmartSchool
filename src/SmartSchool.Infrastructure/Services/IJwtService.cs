using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartSchool.Infrastructure.Services;

public interface IJwtService
{
    string GenerateJwtToken(Guid userId, string email, string role, out DateTime expiresAt);
    string GenerateRefreshToken();
}


public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) => _config = config;

    public string GenerateJwtToken(Guid userId, string email, string role, out DateTime expiresAt)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = jwtSection["Key"] ?? throw new Exception("Key not configured");
        var issuer = jwtSection["Issuer"] ?? throw new Exception("Issuer not configured");
        var audience = jwtSection["Audience"] ?? throw new Exception("Audience not configured");
        var expirationMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var minutes) ? minutes : 60;

        //Claims, Signing Credentials, Token Creation logic goes here
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keySec = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(keySec, SecurityAlgorithms.HmacSha256);


        expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        //Token Creation
        var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            NotBefore = DateTime.UtcNow,
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = creds
        });

        return token;
    }

    public string GenerateRefreshToken()
    {
        // 64 bytes base64 string
        var random = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(random);
    }
}