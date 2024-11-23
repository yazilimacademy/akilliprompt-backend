using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class Placeholder : EntityBase
{
    public string Name { get; set; }
    public Guid PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public static Placeholder Create(string name, Guid promptId)
    {
        return new Placeholder
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            PromptId = promptId
        };
    }
}
