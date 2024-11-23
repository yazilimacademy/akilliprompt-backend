using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Delete;

public sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly ApplicationDbContext _dbContext;
    public DeleteCategoryValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Lutfen bir kategori seciniz.");

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
