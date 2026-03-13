using System.Reflection;
using Microsoft.OpenApi.Models;

namespace UserApi.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "UserApi",
                Version = "v1",
                Description = "REST API for user management built with .NET 8, Clean Architecture, DDD, and CQRS.",
                Contact = new OpenApiContact
                {
                    Name = "Development Team"
                }
            });

            foreach (var xmlFile in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
            }

            options.EnableAnnotations();
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserApi v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
