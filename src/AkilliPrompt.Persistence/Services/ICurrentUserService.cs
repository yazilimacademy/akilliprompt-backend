using System;

namespace AkilliPrompt.Persistence.Services;

public interface ICurrentUserService
{
    long? UserId();
}
