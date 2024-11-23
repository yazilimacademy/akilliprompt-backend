using AkilliPrompt.Persistence.EntityFramework.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetById;

public sealed class GetByIdCategoryQueryHandler : IRequestHandler<GetByIdCategoryQuery, GetByIdCategoryDto>
{
    private readonly ApplicationDbContext _dbContext;
    public GetByIdCategoryQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetByIdCategoryDto> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
    {
        return _dbContext
        .Categories
        .AsNoTracking()
        .Where(x => x.Id == request.Id)
        .Select(x => new GetByIdCategoryDto(x.Id, x.Name, x.Description))
        .FirstOrDefaultAsync(cancellationToken);
    }
}
