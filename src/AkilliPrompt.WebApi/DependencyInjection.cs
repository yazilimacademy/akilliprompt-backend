using AkilliPrompt.Persistence.Services;
using AkilliPrompt.WebApi.Configuration;
using AkilliPrompt.WebApi.Services;

namespace AkilliPrompt.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
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

        return services;
    }
}
