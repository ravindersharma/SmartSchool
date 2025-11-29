using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Common.Mappings;
using System.Reflection;

namespace SmartSchool.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        

        var config= MapsterConfig.CreateConfig();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
        return services;
    }

}
