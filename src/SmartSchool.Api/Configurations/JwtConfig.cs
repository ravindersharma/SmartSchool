using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartSchool.Api.Configurations
{
    /// <summary>
    /// JWT Authentication configuration helper.
    /// Creates TokenValidationParameters for JwtBearer authentication.
    /// </summary>
    public static class JwtConfig
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration config)
        {
            var jwtSection = config.GetSection("Jwt"); ;

            var key = jwtSection["Key"] ?? throw new Exception("Key not configured");
            var issuer = config["Jwt:Issuer"] ?? throw new Exception("Issuer not configured");
            var audience = jwtSection["Audience"] ?? throw new Exception("Audience not configured");


            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime =  true,
                ValidateIssuerSigningKey = true,


                ValidIssuer = issuer,
                ValidAudience = audience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        
                ClockSkew = TimeSpan.Zero
            };

        }
    }
}
