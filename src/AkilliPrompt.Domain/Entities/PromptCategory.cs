using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class PromptCategory : EntityBase
{
    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public long CategoryId { get; set; }
    public Category Category { get; set; }

    public static PromptCategory Create(long promptId, long categoryId)
    {
        return new PromptCategory
        {
            Id = TsidCreator.GetTsid().ToLong(),
            PromptId = promptId,
            CategoryId = categoryId,
        };
    }
}
