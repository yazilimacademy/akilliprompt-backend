using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AkilliPrompt.WebApi.Filters;

public sealed class SwaggerJsonIgnoreFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        List<PropertyInfo> ignoredProperties = context.MethodInfo.GetParameters()
            .SelectMany(p => p.ParameterType.GetProperties()
            .Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null))
            .ToList();

        if (!ignoredProperties.Any())
        {
            return;
        }

        foreach (PropertyInfo property in ignoredProperties)
        {
            operation.Parameters = operation.Parameters
                .Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture))
                .ToList();
        }
    }
}