using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Infrastructure.Repositories;
using SmartSchool.Infrastructure.Services;
namespace SmartSchool.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {

        var conn = config.GetConnectionString("SqliteConnection") ?? "Data Source=smartscholl.db";
        // Register infrastructure services here
        services.AddDbContext<SchoolDbContext>(options =>
            options.UseSqlite(conn));

        services.AddScoped<IStudentRepository, StudentRepository>();

        //Auth Repo + Services can be registered here
        services.AddScoped<IUserRspository, UserRepository>();
        services.AddScoped<IRefreshTokenRespository, RefreshTokenRespository>();
        services.AddScoped<IPasswordResetTokenRespository, PasswordResetTokenRespository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }

}
