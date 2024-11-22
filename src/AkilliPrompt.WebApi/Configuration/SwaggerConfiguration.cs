using AkilliPrompt.WebApi.Options;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.Filters;

namespace AkilliPrompt.WebApi.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerWithVersion(this IServiceCollection services)
    {
        services.AddSwaggerGen();

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
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Sucrose API {description.GroupName.ToUpperInvariant()}");
            }
        });

        return app;
    }
}
