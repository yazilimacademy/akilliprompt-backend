using AkilliPrompt.Domain.Common;

namespace AkilliPrompt.Domain.Entities;

public sealed class PromptCategory : EntityBase
{
    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public long CategoryId { get; set; }
    public Category Category { get; set; }
}
