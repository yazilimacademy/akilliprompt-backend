using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.PromptComments.Queries.GetAll;

public sealed class GetAllPromptCommentsQueryHandler : IRequestHandler<GetAllPromptCommentsQuery, PaginatedList<GetAllPromptCommentsDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllPromptCommentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetAllPromptCommentsDto>> Handle(GetAllPromptCommentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PromptComments.AsQueryable();

        query = query.Where(pc => pc.PromptId == request.PromptId);

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderByDescending(pc => pc.CreatedAt);

        query = query.Include(pc => pc.User);

        query = query.Include(pc => pc.ParentComment);

        query = query.Include(pc => pc.ChildComments);

        var items = await query
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var dtos = items.Select(pc => new GetAllPromptCommentsDto(pc)).ToList();

        return PaginatedList<GetAllPromptCommentsDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
