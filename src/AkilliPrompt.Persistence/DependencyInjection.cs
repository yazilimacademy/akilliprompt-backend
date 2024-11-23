using System;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AkilliPrompt.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__ef_migrations_history")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.PendingModelChangesWarning)));

        return services;
    }
}
