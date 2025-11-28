using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
        return services;
    }

}
