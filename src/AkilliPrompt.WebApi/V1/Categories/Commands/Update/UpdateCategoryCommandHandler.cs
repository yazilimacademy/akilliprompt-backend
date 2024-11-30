using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Update;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseDto<Guid>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheInvalidator _cacheInvalidator;
    public UpdateCategoryCommandHandler(ApplicationDbContext dbContext, CacheInvalidator cacheInvalidator)
    {
        _dbContext = dbContext;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<ResponseDto<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await _dbContext
            .Categories
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, request.Name)
            .SetProperty(x => x.Description, request.Description), cancellationToken);

        // Invalidate relevant caches concurrently
        await _cacheInvalidator.InvalidateGroupAsync("Categories", cancellationToken);

        return ResponseDto<Guid>.Success(request.Id, MessageHelper.GetApiSuccessUpdatedMessage("Kategori"));
    }
}
