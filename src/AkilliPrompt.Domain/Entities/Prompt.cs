using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class Prompt : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Content { get; private set; }

    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public int LikeCount { get; private set; }

    public Guid CreatorId { get; private set; }
    public ApplicationUser Creator { get; private set; }

    public ICollection<PromptCategory> PromptCategories { get; private set; } = [];
    public ICollection<UserFavoritePrompt> UserFavoritePrompts { get; set; } = [];
    public ICollection<UserLikePrompt> UserLikePrompts { get; set; } = [];
    public ICollection<Placeholder> Placeholders { get; set; } = [];
    public ICollection<PromptComment> PromptComments { get; set; } = [];


    public static Prompt Create(string title, string description, string content, bool isActive, Guid creatorId)
    {
        return new Prompt
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Description = description,
            Content = content,
            IsActive = isActive,
            CreatorId = creatorId
        };
    }

    public void SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public void Update(string title, string description, string content, bool isActive)
    {
        Title = title;
        Description = description;
        Content = content;
        IsActive = isActive;
    }
}
