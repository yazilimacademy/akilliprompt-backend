namespace AkilliPrompt.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class CacheOptionsAttribute : Attribute
{
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; }
    public TimeSpan? SlidingExpiration { get; }

    public CacheOptionsAttribute(double absoluteExpirationMinutes = 30, double slidingExpirationMinutes = 10)
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationMinutes);
        SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes);
    }
}
