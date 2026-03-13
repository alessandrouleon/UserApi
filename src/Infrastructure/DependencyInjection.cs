using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApication.Interfaces;
using UserApin.Interfaces.Repositories;
using UserApistructure.Persistence.Context;
using UserApistructure.Persistence.Repositories;
using UserApistructure.Services;

namespace UserApi.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Registers all Infrastructure-layer services: EF Core, repositories, and application services.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHashService, HashService>();

        return services;
    }
}
