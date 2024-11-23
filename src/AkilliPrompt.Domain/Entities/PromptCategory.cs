using AkilliPrompt.Domain.Common;
using TSID.Creator.NET;

namespace AkilliPrompt.Domain.Entities;

public sealed class PromptCategory : EntityBase
{
    public Guid PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public static PromptCategory Create(Guid promptId, Guid categoryId)
    {
        return new PromptCategory
        {
            Id = Guid.CreateVersion7(),
            PromptId = promptId,
            CategoryId = categoryId,
        };
    }
}
