using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class Prompt : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Content { get; private set; }

    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<PromptCategory> PromptCategories { get; private set; } = [];
    public ICollection<UserFavoritePrompt> UserFavoritePrompts { get; set; } = [];
    public ICollection<UserLikePrompt> UserLikePrompts { get; set; } = [];
    public ICollection<Placeholder> Placeholders { get; set; } = [];


    public static Prompt Create(string title, string description, string content, bool isActive)
    {
        return new Prompt
        {
            Id = TsidCreator.GetTsid().ToLong(),
            Title = title,
            Description = description,
            Content = content,
            IsActive = isActive
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
