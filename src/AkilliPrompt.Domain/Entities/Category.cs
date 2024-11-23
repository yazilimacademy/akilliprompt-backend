using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class Category : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public ICollection<PromptCategory> PromptCategories { get; set; } = [];

    public static Category Create(string name, string description)
    {
        return new Category
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Description = description,
        };
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
