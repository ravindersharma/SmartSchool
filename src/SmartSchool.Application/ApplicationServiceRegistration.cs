using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Behaviors;
using SmartSchool.Application.Common.Mappings;
using System.Reflection;

namespace SmartSchool.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        var config = MapsterConfig.CreateConfig();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        //Automatice validation behavior registeration 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        //loggin pipeline behavior can be added here
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }

}
