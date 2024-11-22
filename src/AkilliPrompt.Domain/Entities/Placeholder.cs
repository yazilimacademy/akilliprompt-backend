using AkilliPrompt.Domain.Common;

namespace AkilliPrompt.Domain.Entities;

public sealed class Placeholder : EntityBase
{
    public string Name { get; set; }
    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }
}
