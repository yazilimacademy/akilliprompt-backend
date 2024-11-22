using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class Placeholder : EntityBase
{
    public string Name { get; set; }
    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public static Placeholder Create(string name, long promptId)
    {
        return new Placeholder
        {
            Id = TsidCreator.GetTsid().ToLong(),
            Name = name,
            PromptId = promptId
        };
    }
}
