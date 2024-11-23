using System;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Update;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ApplicationDbContext _dbContext;
    public UpdateCategoryCommandValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Lutfen bir kategori seciniz.");

        RuleFor(x => x.Id)
            .MustAsync(IsCategoryExistsAsync)
            .WithMessage("Kategori bulunamadı.");

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

        RuleFor(x => x)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("Bu ada sahip bir kategori zaten mevcuttur.");
    }

    private async Task<bool> BeUniqueNameAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        return !await _dbContext
        .Categories
        .AnyAsync(x => x.Id != command.Id && string.Equals(x.Name, command.Name, StringComparison.OrdinalIgnoreCase), cancellationToken);
    }

    private Task<bool> IsCategoryExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext
        .Categories
        .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
