namespace AkilliPrompt.WebApi.Services;

public interface ICacheInvalidator
{
    Task InvalidateAsync(string cacheKey, CancellationToken cancellationToken);
}
