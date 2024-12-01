using AkilliPrompt.Domain.Helpers;
using AkilliPrompt.Persistence.Services;

namespace AkilliPrompt.WebApi.Services;

public sealed class CurrentUserManager : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;

    public CurrentUserManager(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }

    // public long? UserId => GetUserId();
    public long? UserId => 123456;

    public string IpAddress => GetIpAddress();

    private long? GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("uid")?.Value;

        return userId is null ? null : long.Parse(userId);
    }

    private string GetIpAddress()
        {
            if (_env.IsDevelopment())
                return IpHelper.GetIpAddress();

            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            else
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
}
