namespace AkilliPrompt.WebApi.V1.Models.Prompts;

public sealed record CreatePromptDto(
    string Title,
    string Description,
    string Content,
    IFormFile? Image,
    bool IsActive,
    List<long> CategoryIds,
    List<string> PlaceholderNames);