using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Delete;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseDto<Guid>>
{
    private readonly ApplicationDbContext _dbContext;
    public DeleteCategoryCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _dbContext
            .Categories
            .Where(x => x.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        return ResponseDto<Guid>.Success(MessageHelper.GetApiSuccessDeletedMessage("Kategori"));
    }
}
