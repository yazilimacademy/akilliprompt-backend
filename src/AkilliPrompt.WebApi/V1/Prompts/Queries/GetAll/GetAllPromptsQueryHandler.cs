using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetAll;

public sealed class GetAllPromptsQueryHandler : IRequestHandler<GetAllPromptsQuery, PaginatedList<GetAllPromptsDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllPromptsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetAllPromptsDto>> Handle(GetAllPromptsQuery request, CancellationToken cancellationToken)
    {
        // Base query with filters
        var query = _context.Prompts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            query = query.Where(p => p.Title.Contains(request.SearchKeyword));

        if (request.CategoryIds.Any())
            query = query.Where(p => p.PromptCategories.Any(c => request.CategoryIds.Contains(c.CategoryId)));

        // Order by LikeCount descending
        query = query.OrderByDescending(p => p.LikeCount);

        // Get total count after filters but before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var items = await query
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => GetAllPromptsDto.Create(
                p.Id,
                p.Title,
                p.Description,
                p.Content,
                p.ImageUrl))
            .ToListAsync(cancellationToken);

        return PaginatedList<GetAllPromptsDto>.Create(items, totalCount, request.PageNumber, request.PageSize);
    }
}

