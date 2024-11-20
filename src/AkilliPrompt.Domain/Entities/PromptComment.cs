using System;

namespace AkilliPrompt.Domain.Entities;

public sealed class PromptComment : EntityBase
{
    public string Content { get; set; }

    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }
}
