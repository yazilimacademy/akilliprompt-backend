using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Create;

public sealed class CreatePromptCommand : IRequest<ResponseDto<Guid>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public IFormFile? Image { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> CategoryIds { get; set; }
    public List<string> PlaceholderNames { get; set; }

    public CreatePromptCommand(string title, string description, string content, IFormFile? image, bool isActive, List<Guid> categoryIds, List<string> placeholderNames)
    {
        Title = title;
        Description = description;
        Content = content;
        Image = image;
        IsActive = isActive;
        CategoryIds = categoryIds ?? [];
        PlaceholderNames = placeholderNames ?? [];
    }

    public CreatePromptCommand()
    {

    }

}