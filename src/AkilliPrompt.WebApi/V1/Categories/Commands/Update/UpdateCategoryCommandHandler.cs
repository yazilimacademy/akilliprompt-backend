using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Update;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseDto<Guid>>
{
    private readonly ApplicationDbContext _dbContext;
    public UpdateCategoryCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await _dbContext
            .Categories
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, request.Name)
            .SetProperty(x => x.Description, request.Description), cancellationToken);

        // var category = await _dbContext
        //     .Categories
        //     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        // category.Update(request.Name, request.Description);

        // await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseDto<Guid>.Success(request.Id, MessageHelper.GetApiSuccessUpdatedMessage("Kategori"));
    }
}
