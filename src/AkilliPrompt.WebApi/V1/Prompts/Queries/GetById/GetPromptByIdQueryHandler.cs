using AkilliPrompt.Persistence.EntityFramework.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetById;

public sealed class GetPromptByIdQueryHandler : IRequestHandler<GetPromptByIdQuery, GetPromptByIdDto>
{
    private readonly ApplicationDbContext _context;
    public GetPromptByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<GetPromptByIdDto> Handle(GetPromptByIdQuery request, CancellationToken cancellationToken)
    {
        var prompt = await _context
        .Prompts
        .Include(x => x.PromptCategories)
            .ThenInclude(x => x.Category)
        .Include(x => x.Placeholders)
        .AsSplitQuery()
        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        return new GetPromptByIdDto(prompt.Id, prompt.Title, prompt.Description, prompt.Content, prompt.ImageUrl, prompt.IsActive, prompt.PromptCategories.Select(c => new PromptCategoryDto(c.Id, c.Category.Name)), prompt.Placeholders.Select(ph => new PlaceholderDto(ph.Id, ph.Name)));
    }
}
