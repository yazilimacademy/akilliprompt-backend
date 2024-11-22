using AkilliPrompt.Persistence.Services;
using AkilliPrompt.WebApi.Services;

namespace AkilliPrompt.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserManager>();

        return services;
    }
}
