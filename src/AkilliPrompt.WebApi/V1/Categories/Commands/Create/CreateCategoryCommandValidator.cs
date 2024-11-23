using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ApplicationDbContext _dbContext;
    public CreateCategoryCommandValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Kategori adı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("Kategori adı en fazla 100 karakter olabilir.")
            .MinimumLength(2)
            .WithMessage("Kategori adı en az 2 karakter olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Kategori açıklaması boş olamaz.")
            .MaximumLength(500)
            .WithMessage("Kategori açıklaması en fazla 500 karakter olabilir.")
            .MinimumLength(2)
            .WithMessage("Kategori açıklaması en az 2 karakter olmalıdır.");

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("Bu ada sahip bir kategori zaten mevcuttur.");
    }

    private async Task<bool> BeUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        return !await _dbContext
        .Categories
        .AnyAsync(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase), cancellationToken);
    }
}
