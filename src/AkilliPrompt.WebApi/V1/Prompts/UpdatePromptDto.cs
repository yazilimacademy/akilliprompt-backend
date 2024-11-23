namespace AkilliPrompt.WebApi.V1.Prompts;

public sealed record UpdatePromptDto(
    Guid Id,
    string Title,
    string Description,
    string Content,
    IFormFile? Image,
    bool IsActive,
    ICollection<Guid> CategoryIds,
    ICollection<string> PlaceholderNames);