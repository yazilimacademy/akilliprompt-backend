namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetAll;

public sealed record GetAllPromptsDto
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Content { get; private set; }
    public string? ImageUrl { get; private set; }

    private GetAllPromptsDto(Guid id, string title, string description, string content, string? imageUrl)
    {
        Id = id;
        Title = title;
        Description = description;
        Content = content;
        ImageUrl = imageUrl;
    }

    public static GetAllPromptsDto Create(Guid id, string title, string description, string content, string? imageUrl)
    {
        return new GetAllPromptsDto(id, title, description, content, imageUrl);
    }
}
