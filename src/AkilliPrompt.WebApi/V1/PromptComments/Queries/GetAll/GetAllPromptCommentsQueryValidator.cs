using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.PromptComments.Queries.GetAll;

public class GetAllPromptCommentsQueryValidator : AbstractValidator<GetAllPromptCommentsQuery>
{
    private readonly ApplicationDbContext _context;

    public GetAllPromptCommentsQueryValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(e => e.PromptId)
            .NotEmpty()
            .WithMessage("Lütfen geçerli bir prompt seçiniz.");

        RuleFor(e => e.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Lütfen geçerli bir sayfa numarası seçiniz.");

        RuleFor(e => e.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Lütfen geçerli bir sayfa boyutu seçiniz.");

        RuleFor(e => e.PromptId)
            .MustAsync(PromptExists)
            .WithMessage("Belirtilen prompt mevcut değil.");
    }

    private Task<bool> PromptExists(Guid promptId, CancellationToken cancellationToken)
    {
        return _context
        .Prompts
        .AnyAsync(p => p.Id == promptId, cancellationToken);
    }
}
