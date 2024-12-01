using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.Persistence.Services;
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
    private readonly ICurrentUserService _currentUserService;

    public CreatePromptCommandHandler(ApplicationDbContext context, CacheInvalidator cacheInvalidator, R2ObjectStorageManager r2ObjectStorageManager, ICurrentUserService currentUserService)
    {
        _context = context;
        _cacheInvalidator = cacheInvalidator;
        _r2ObjectStorageManager = r2ObjectStorageManager;
        _currentUserService = currentUserService;
    }


    public async Task<ResponseDto<Guid>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        var prompt = Prompt.Create(request.Title, request.Description, request.Content, request.IsActive, _currentUserService.UserId);

        if (request.Image is not null)
        {
            var imageUrl = await _r2ObjectStorageManager.UploadPromptPicAsync(request.Image, cancellationToken);

            prompt.SetImageUrl(imageUrl);
        }

        if (request.CategoryIds.Any())
        {
            foreach (var categoryId in request.CategoryIds)
            {
                var promptCategory = PromptCategory.Create(prompt.Id, categoryId);

                _context.PromptCategories.Add(promptCategory);
            }
        }

        if (request.PlaceholderNames.Any())
        {
            foreach (var placeholderName in request.PlaceholderNames)
            {
                var placeholder = Placeholder.Create(placeholderName, prompt.Id);

                _context.Placeholders.Add(placeholder);
            }
        }

        _context.Prompts.Add(prompt);

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheInvalidator.InvalidateGroupAsync("Prompts", cancellationToken);

        return ResponseDto<Guid>.Success(prompt.Id, MessageHelper.GetApiSuccessCreatedMessage("Prompt"));
    }
}
