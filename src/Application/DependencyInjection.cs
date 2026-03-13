using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserApication.Behaviors;
using UserApication.Mappings;

namespace UserApication;

public static class DependencyInjection
{
    /// <summary>Registers all Application-layer services: MediatR, AutoMapper, FluentValidation, and pipeline behaviors.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
