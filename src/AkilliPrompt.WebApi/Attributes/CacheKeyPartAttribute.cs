namespace AkilliPrompt.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class CacheKeyPartAttribute : Attribute
{
    // Optionally, add properties to control behavior, such as encoding
    public bool Encode { get; set; } = true;

    public string Prefix { get; set; } = string.Empty;
}
