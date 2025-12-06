using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Application.Common.Interfaces;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Application.Users.Interfaces;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Infrastructure.Repositories;
using SmartSchool.Infrastructure.Services.Auth;
using SmartSchool.Infrastructure.Services.Email;
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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRespository, RefreshTokenRespository>();
        services.AddScoped<IPasswordResetTokenRespository, PasswordResetTokenRespository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        bool useStub = config.GetSection("UseStubEmail").Value == "true";
        bool useSendGrid = config.GetSection("UseSendGrid").Value == "true";

        // Email Queue 
        services.AddSingleton<IEmailQueue, EmailQueue>();

        //Template service
        services.AddSingleton<IEmailTemplateService, EmailTemplateService>();

        //Emal Sender can be registered here based on config
        if (useStub)
        {
            services.AddSingleton<IEmailSender, EmailSenderStub>();
        }
        else if (useSendGrid)
        {
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
        }
        else
        {
            services.AddSingleton<IEmailSender, SmtpEmailSender>();
        }

    

        return services;
    }

}
