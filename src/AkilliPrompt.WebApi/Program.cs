using Azure.Identity;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["AzureKeyVaultSettings:Uri"]),
    new ClientSecretCredential(
        tenantId: builder.Configuration["AzureKeyVaultSettings:TenantId"],
        clientId: builder.Configuration["AzureKeyVaultSettings:ClientId"],
        clientSecret: builder.Configuration["AzureKeyVaultSettings:ClientSecret"]
    ));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
