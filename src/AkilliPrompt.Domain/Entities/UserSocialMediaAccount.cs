using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Enums;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class UserSocialMediaAccount : EntityBase
{
    public SocialMediaType SocialMediaType { get; set; }
    public string Url { get; set; }

    public long UserId { get; set; }
    public ApplicationUser User { get; set; }
}
