using AkilliPrompt.WebApi.Options;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AkilliPrompt.WebApi.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerWithVersion(this IServiceCollection services)
    {
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = $"Input your Bearer token in this format - Bearer token to access this API",
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
        });

        services.
            AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); //ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("X-Api-Version"), new MediaTypeApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        services.AddSwaggerExamplesFromAssemblyOf<Program>();

        return services;
    }


    public static IApplicationBuilder UseSwaggerWithVersion(this IApplicationBuilder app)
    {
        IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"AkilliPrompt API {description.GroupName.ToUpperInvariant()}");
            }
        });

        return app;
    }
}
