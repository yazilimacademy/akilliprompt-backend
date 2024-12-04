using AkilliPrompt.Domain.Entities;
using AkilliPrompt.WebApi.Services;
using FluentValidation;

namespace AkilliPrompt.WebApi.V1.PromptComments.Queries.GetAll;

public class GetAllPromptCommentsQueryValidator : AbstractValidator<GetAllPromptCommentsQuery>
{
    private readonly IExistenceService<Prompt> _existenceService;

    public GetAllPromptCommentsQueryValidator(IExistenceService<Prompt> existenceService)
    {
        _existenceService = existenceService;

        RuleFor(e => e.PageNumber)
            .InclusiveBetween(1, 50)
            .WithMessage("Lütfen 1 ile 50 arasında bir sayfa numarası seçiniz.");

        RuleFor(e => e.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("Lütfen 1 ile 50 arasında bir sayfa boyutu seçiniz.");

        RuleFor(e => e.PromptId)
        .NotEmpty()
            .WithMessage("Lütfen geçerli bir prompt seçiniz.")
            .MustAsync(PromptExists)
            .WithMessage("Belirtilen prompt mevcut değil.");
    }

    private Task<bool> PromptExists(Guid promptId, CancellationToken cancellationToken)
    {
        return _existenceService.ExistsAsync(promptId, cancellationToken);
    }
}
