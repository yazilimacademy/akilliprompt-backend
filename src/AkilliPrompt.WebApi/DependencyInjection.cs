using AkilliPrompt.Domain.Settings;
using AkilliPrompt.Persistence.Services;
using AkilliPrompt.WebApi.Configuration;
using AkilliPrompt.WebApi.Services;

namespace AkilliPrompt.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerWithVersion();
        services.AddEndpointsApiExplorer();

        services.AddMemoryCache();

        services.AddProblemDetails();
        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
            });

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserManager>();

        services.Configure<CloudflareR2Settings>(
            configuration.GetSection(nameof(CloudflareR2Settings)));


        // Scoped Services
        services.AddScoped<R2ObjectStorageManager>();

        return services;
    }
}
