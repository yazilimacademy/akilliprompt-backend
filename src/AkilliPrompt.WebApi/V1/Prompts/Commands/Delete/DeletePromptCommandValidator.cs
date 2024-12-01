using AkilliPrompt.Domain.Entities;
using AkilliPrompt.WebApi.Common;
using AkilliPrompt.WebApi.Services;
using FluentValidation;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Delete;

public sealed class DeletePromptCommandValidator : EntityExistsValidator<Prompt, DeletePromptCommand>
{

    public DeletePromptCommandValidator(IExistenceService<Prompt> existenceService)
        : base(existenceService)
    {
        RuleFor(p => p.Id)
        .NotEmpty()
        .WithMessage("Lütfen bir prompt seçiniz.");

    }

    protected override Guid GetEntityId(DeletePromptCommand command) => command.Id;
}
