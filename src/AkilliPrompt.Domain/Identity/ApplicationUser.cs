using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AkilliPrompt.Domain.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>, ICreatedByEntity, IModifiedByEntity
{
    public FullName FullName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedByUserId { get; set; }

    public string? ModifiedByUserId { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    public ICollection<UserSocialMediaAccount> UserSocialMediaAccounts { get; set; } = [];
    public ICollection<PromptComment> PromptComments { get; set; } = [];
    public ICollection<UserFavoritePrompt> UserFavoritePrompts { get; set; } = [];
    public ICollection<UserLikePrompt> UserLikePrompts { get; set; } = [];
    public ICollection<Prompt> CreatedPrompts { get; set; } = [];

    public static ApplicationUser Create(string email, FullName fullName, bool isEmailConfirmed = false)
    {
        return new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            UserName = email,
            FullName = fullName,
            EmailConfirmed = isEmailConfirmed,
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }
}
