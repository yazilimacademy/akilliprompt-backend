using System;
using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;

public sealed class UserPromptComment : EntityBase
{
    public int Level { get; set; }
    public string Content { get; set; }

    public long PromptId { get; set; }
    public Prompt Prompt { get; set; }

    public long UserId { get; set; }
    public ApplicationUser User { get; set; }

    public long? ParentCommentId { get; set; }
    public UserPromptComment ParentComment { get; set; }

    public ICollection<UserPromptComment> ChildComments { get; set; } = [];
}
