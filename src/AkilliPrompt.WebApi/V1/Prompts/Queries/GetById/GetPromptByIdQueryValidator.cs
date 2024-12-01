using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetById;

public sealed class GetPromptByIdQueryValidator : AbstractValidator<GetPromptByIdQuery>
{
    private readonly ApplicationDbContext _context;
    public GetPromptByIdQueryValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.Id)
        .NotEmpty()
        .WithMessage("Lütfen bir prompt seçiniz.")
        .MustAsync(IsPromptExistsAsync)
        .WithMessage("Belirtilen id'ye sahip prompt bulunamadı.");
    }

    private Task<bool> IsPromptExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Prompts.AnyAsync(p => p.Id == id, cancellationToken);
    }

}
