using Azure.Identity;
using AkilliPrompt.WebApi;
using AkilliPrompt.Persistence;
using Serilog;
using AkilliPrompt.WebApi.Configuration;
using AkilliPrompt.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Add services to the container.

    if (!builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddAzureKeyVault(
       new Uri(builder.Configuration["AzureKeyVaultSettings:Uri"]),
       new ClientSecretCredential(
           tenantId: builder.Configuration["AzureKeyVaultSettings:TenantId"],
           clientId: builder.Configuration["AzureKeyVaultSettings:ClientId"],
           clientSecret: builder.Configuration["AzureKeyVaultSettings:ClientSecret"]
       ));
    }

    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddWebApi(builder.Configuration);

    // Suppress model state validation suppression
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    builder.Services.AddControllers();


    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();


    app.UseCors("AllowAll");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerWithVersion();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.ApplyMigrations();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
