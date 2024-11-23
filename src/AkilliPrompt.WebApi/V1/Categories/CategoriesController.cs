using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.V1.Categories.Commands.Create;
using AkilliPrompt.WebApi.V1.Categories.Commands.Delete;
using AkilliPrompt.WebApi.V1.Categories.Commands.Update;
using AkilliPrompt.WebApi.V1.Categories.Queries.GetAll;
using AkilliPrompt.WebApi.V1.Categories.Queries.GetById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace AkilliPrompt.WebApi.V1.Categories;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _mediator;

    public CategoriesController(
        ApplicationDbContext dbContext,
        ISender mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetByIdCategoryQuery(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (command.Id != id)
            return BadRequest();

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken));
    }
}