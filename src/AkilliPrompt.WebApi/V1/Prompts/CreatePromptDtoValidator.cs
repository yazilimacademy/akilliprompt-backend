using FluentValidation;

namespace AkilliPrompt.WebApi.V1.Prompts;

public sealed class CreatePromptDtoValidator : AbstractValidator<CreatePromptDto>
{
    public CreatePromptDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Başlık alanı boş bırakılamaz.")
            .MaximumLength(200)
            .WithMessage("Başlık alanı en fazla {1} karakter olabilir.")
            .MinimumLength(3)
            .WithMessage("Başlık alanı en az {1} karakter olmalıdır.");
    }

}
