namespace AkilliPrompt.WebApi.V1.Models.Prompts;

public sealed record GetByIdPromptDto(
    long Id,
    string Title,
    string Description,
    string Content,
    string? ImageUrl,
    bool IsActive,
    ICollection<PromptCategoryDto> Categories,
    ICollection<PlaceholderDto> Placeholders);

public sealed record PromptCategoryDto(long Id, string Name);
public sealed record PlaceholderDto(long Id, string Name); 