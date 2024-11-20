using AkilliPrompt.Domain.Common;

namespace AkilliPrompt.Domain.Entities;

public sealed class Prompt : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }

    public ICollection<PromptCategory> PromptCategories { get; set; } = [];
    public ICollection<UserFavoritePrompt> UserFavoritePrompts { get; set; } = [];
    public ICollection<UserLikePrompt> UserLikePrompts { get; set; } = [];
    public ICollection<Placeholder> Placeholders { get; set; } = [];
}
