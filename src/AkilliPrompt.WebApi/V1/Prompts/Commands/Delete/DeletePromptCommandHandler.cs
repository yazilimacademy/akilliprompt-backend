using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Delete;

public sealed class DeletePromptCommandHandler : IRequestHandler<DeletePromptCommand, ResponseDto<Guid>>
{
    private readonly IExistenceService<Prompt> _existenceService;
    private readonly CacheInvalidator _cacheInvalidator;

    private readonly ApplicationDbContext _context;

    public DeletePromptCommandHandler(
        IExistenceService<Prompt> existenceService,
        CacheInvalidator cacheInvalidator,
        ApplicationDbContext context)
    {
        _existenceService = existenceService;
        _cacheInvalidator = cacheInvalidator;
        _context = context;
    }

    public async Task<ResponseDto<Guid>> Handle(DeletePromptCommand request, CancellationToken cancellationToken)
    {
        await _context
        .Prompts
        .Where(p => p.Id == request.Id)
        .ExecuteDeleteAsync(cancellationToken);

        await _cacheInvalidator.InvalidateGroupAsync("Prompts", cancellationToken);

        await _existenceService.RemoveExistenceAsync(request.Id, cancellationToken);

        return ResponseDto<Guid>.Success(request.Id, MessageHelper.GetApiSuccessDeletedMessage(typeof(Prompt).Name));
    }
}
