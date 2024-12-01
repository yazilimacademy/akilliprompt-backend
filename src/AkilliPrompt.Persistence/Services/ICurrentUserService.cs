using System;

namespace AkilliPrompt.Persistence.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string IpAddress { get; }
}
