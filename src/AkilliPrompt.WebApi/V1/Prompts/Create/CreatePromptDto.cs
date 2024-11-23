namespace AkilliPrompt.WebApi.V1.Prompts.Create;

public sealed record CreatePromptDto(
    string Title,
    string Description,
    string Content,
    IFormFile? Image,
    bool IsActive,
    List<Guid> CategoryIds,
    List<string>? PlaceholderNames = null
);