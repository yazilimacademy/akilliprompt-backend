using AkilliPrompt.Domain.Common;

namespace AkilliPrompt.Domain.Entities;

public sealed class Category : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<PromptCategory> PromptCategories { get; set; } = [];
}
