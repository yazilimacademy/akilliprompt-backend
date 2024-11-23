using System.Reflection;
using AkilliPrompt.Domain.Identity;
using AkilliPrompt.Domain.Settings;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.Persistence.Services;
using AkilliPrompt.WebApi.Behaviors;
using AkilliPrompt.WebApi.Configuration;
using AkilliPrompt.WebApi.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

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

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        });

        // Configure Dragonfly as the caching provider
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Dragonfly");
            options.InstanceName = "AkilliPrompt_"; // Optional: Use a specific instance name
                                                    // Add any Dragonfly-specific configurations here
                                                    // For example, if Dragonfly supports specific features or optimizations, configure them here
        });

        services.AddScoped<ICacheInvalidator, CacheInvalidator>();

        return services;
    }

}
