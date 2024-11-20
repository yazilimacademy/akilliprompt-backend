using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class UserFavoritePrompt : EntityBase
{
    public long UserId { get; set; }
    public ApplicationUser User { get; set; }

    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }
}
