namespace AkilliPrompt.WebApi.V1.Models.Prompts;

public sealed record GetAllPromptsDto(
    long Id,
    string Title,
    string Description,
    string? ImageUrl,
    bool IsActive); 