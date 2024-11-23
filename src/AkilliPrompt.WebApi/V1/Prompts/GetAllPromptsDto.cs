namespace AkilliPrompt.WebApi.V1.Prompts;

public sealed record GetAllPromptsDto(
    Guid Id,
    string Title,
    string Description,
    string? ImageUrl,
    bool IsActive);