using System;
using FluentValidation;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetAll;

public sealed class GetAllPromptsQueryValidator : AbstractValidator<GetAllPromptsQuery>
{
    public GetAllPromptsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Sayfa numarası 1 veya daha büyük olmalıdır.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.");

        RuleFor(x => x.SearchKeyword)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.SearchKeyword))
            .WithMessage("Arama anahtarı en fazla 100 karakter olabilir.");
    }
}
