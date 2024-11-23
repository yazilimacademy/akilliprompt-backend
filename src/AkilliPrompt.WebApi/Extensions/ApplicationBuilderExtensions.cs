using AkilliPrompt.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();

        return app;
    }
}
