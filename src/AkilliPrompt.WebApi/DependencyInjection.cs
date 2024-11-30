using System.Reflection;
using System.Text;
using AkilliPrompt.Domain.Identity;
using AkilliPrompt.Domain.Settings;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.Persistence.Services;
using AkilliPrompt.WebApi.Behaviors;
using AkilliPrompt.WebApi.Configuration;
using AkilliPrompt.WebApi.Services;
using AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;
using FluentValidation;
using IAPriceTrackerApp.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace AkilliPrompt.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
                               {
                                   options.AddPolicy("AllowAll",
                                       builder => builder
                                           .AllowAnyMethod()
                                           .AllowCredentials()
                                           .SetIsOriginAllowed((host) => true)
                                           .AllowAnyHeader());
                               });

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

        services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

        services.Configure<GoogleAuthSettings>(
            configuration.GetSection(nameof(GoogleAuthSettings)));


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

        services.AddScoped<CacheInvalidator>();

        services.AddSingleton<CacheKeyFactory>();

        // Register Redis connection for advanced operations
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Dragonfly")));

        services.AddScoped<JwtManager>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var secretKey = configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("JwtSettings:SecretKey is not set.");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

}
