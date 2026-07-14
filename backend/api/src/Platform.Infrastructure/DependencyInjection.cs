using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Application.Abstractions;
using Platform.Application.Identity;
using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Persistence;
using Platform.Infrastructure.Time;

namespace Platform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPlatformInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("缺少 ConnectionStrings:Default 配置。");

        services.AddDbContext<PlatformDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUserAccountStore, EfUserAccountStore>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IIdentityTokenVerifier, FirebaseTokenVerifier>();

        return services;
    }
}
