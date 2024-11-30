using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Delete;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseDto<Guid>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheInvalidator _cacheInvalidator;
    public DeleteCategoryCommandHandler(ApplicationDbContext dbContext, CacheInvalidator cacheInvalidator)
    {
        _dbContext = dbContext;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<ResponseDto<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _dbContext
            .Categories
            .Where(x => x.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await _cacheInvalidator.InvalidateGroupAsync(CacheKeysHelper.GetAllCategoriesKey, cancellationToken);

        return ResponseDto<Guid>.Success(MessageHelper.GetApiSuccessDeletedMessage("Kategori"));
    }
}
