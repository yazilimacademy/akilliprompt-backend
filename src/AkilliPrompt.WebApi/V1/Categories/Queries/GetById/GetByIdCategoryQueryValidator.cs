using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetById;

public sealed class GetByIdCategoryQueryValidator : AbstractValidator<GetByIdCategoryQuery>
{
    private readonly ApplicationDbContext _dbContext;
    public GetByIdCategoryQueryValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kategori seçimi zorunludur.");

        RuleFor(x => x.Id)
            .MustAsync(IsCategoryExistsAsync)
            .WithMessage("Kategori bulunamadı.");
    }

    private Task<bool> IsCategoryExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext
        .Categories
        .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
