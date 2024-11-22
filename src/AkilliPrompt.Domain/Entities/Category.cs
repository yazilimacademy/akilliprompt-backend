using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class Category : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<PromptCategory> PromptCategories { get; set; } = [];

    public static Category Create(string name, string description)
    {
        return new Category
        {
            Id = TsidCreator.GetTsid().ToLong(),
            Name = name,
            Description = description,
        };
    }
}
