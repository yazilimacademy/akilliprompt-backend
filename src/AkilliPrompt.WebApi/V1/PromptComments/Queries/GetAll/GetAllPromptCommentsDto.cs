using System;
using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Domain.ValueObjects;

namespace AkilliPrompt.WebApi.V1.PromptComments.Queries.GetAll;

public sealed record GetAllPromptCommentsDto
{
    public int Level { get; set; }
    public string Content { get; set; }

    public Guid PromptId { get; set; }

    public Guid UserId { get; set; }
    public FullName UserFullName { get; set; }

    public Guid? ParentCommentId { get; set; }
    public GetAllPromptCommentsDto? ParentComment { get; set; }

    public List<GetAllPromptCommentsDto> ChildComments { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }

    public GetAllPromptCommentsDto(int level, string content, Guid promptId, Guid userId, FullName userFullName, Guid? parentCommentId, GetAllPromptCommentsDto? parentComment, List<GetAllPromptCommentsDto> childComments, DateTimeOffset createdAt)
    {
        Level = level;
        Content = content;
        PromptId = promptId;
        UserId = userId;
        UserFullName = userFullName;
        ParentCommentId = parentCommentId;
        ParentComment = parentComment;
        ChildComments = childComments;
        CreatedAt = createdAt;
    }

    public GetAllPromptCommentsDto(PromptComment promptComment)
    {
        Level = promptComment.Level;
        Content = promptComment.Content;
        PromptId = promptComment.PromptId;
        UserId = promptComment.UserId;
        UserFullName = promptComment.User.FullName;
        ParentCommentId = promptComment.ParentCommentId;
        ParentComment = promptComment.ParentComment != null ? new GetAllPromptCommentsDto(promptComment.ParentComment) : null;
        ChildComments = promptComment.ChildComments.Select(pc => new GetAllPromptCommentsDto(pc)).ToList();
        CreatedAt = promptComment.CreatedAt;
    }
}
