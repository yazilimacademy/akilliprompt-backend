namespace AkilliPrompt.WebApi.V1.Prompts;

public sealed record GetAllPromptsDto(
    long Id,
    string Title,
    string Description,
    string? ImageUrl,
    bool IsActive);