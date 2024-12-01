using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Services;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Create;

public sealed class CreatePromptCommandHandler : IRequestHandler<CreatePromptCommand, ResponseDto<Guid>>
{
    private readonly ApplicationDbContext _context;
    private readonly CacheInvalidator _cacheInvalidator;
    private readonly R2ObjectStorageManager _r2ObjectStorageManager;

    public CreatePromptCommandHandler(ApplicationDbContext context, CacheInvalidator cacheInvalidator, R2ObjectStorageManager r2ObjectStorageManager)
    {
        _context = context;
        _cacheInvalidator = cacheInvalidator;
        _r2ObjectStorageManager = r2ObjectStorageManager;
    }


    public async Task<ResponseDto<Guid>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        var prompt = Prompt.Create(request.Title, request.Description, request.Content, request.IsActive);

        if (request.Image is not null)
        {
            var imageUrl = await _r2ObjectStorageManager.UploadPromptPicAsync(request.Image, cancellationToken);

            prompt.SetImageUrl(imageUrl);
        }

        _context.Prompts.Add(prompt);

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheInvalidator.InvalidateGroupAsync("Prompts", cancellationToken);

        return ResponseDto<Guid>.Success(prompt.Id, MessageHelper.GetApiSuccessCreatedMessage("Prompt"));
    }
}
