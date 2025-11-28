using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Infrastructure.Repositories;
namespace SmartSchool.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services here
        services.AddDbContext<SchoolDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(SchoolDbContext).Assembly.FullName)));



        services.AddScoped<IStudentRepository, StudentRepository>();
        return services;
    }

}
