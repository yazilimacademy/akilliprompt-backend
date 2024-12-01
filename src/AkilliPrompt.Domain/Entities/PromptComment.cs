using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class PromptComment : EntityBase
{
    public int Level { get; set; }
    public string Content { get; set; }

    public Guid PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid? ParentCommentId { get; set; }
    public PromptComment ParentComment { get; set; }

    public ICollection<PromptComment> ChildComments { get; set; } = [];
}
