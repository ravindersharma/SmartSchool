using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartSchool.Api.Configurations
{
    public static class AuthConfig
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration config)
        {

            var key = config["Jwt:Key"] ?? throw new Exception("Jwt:Key not configured");
            var issuer = config["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer not configured");

            return new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

        }
    }
}
