using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class UserFavoritePrompt : EntityBase
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid PromptId { get; set; }
    public Prompt Prompt { get; set; }
}
