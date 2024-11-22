using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.V1.Models.Categories;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace AkilliPrompt.WebApi.V1.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ApiVersion("1.0")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _dbContext;

    public CategoriesController(IMemoryCache memoryCache, ApplicationDbContext dbContext)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _dbContext
        .Categories
        .AsNoTracking()
        .Select(category => new GetAllCategoriesDto(category.Id, category.Name))
        .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _dbContext
        .Categories
        .AsNoTracking()
        .Select(category => new GetByIdCategoryDto(category.Id, category.Name, category.Description))
        .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = Category.Create(dto.Name, dto.Description);

        _dbContext.Categories.Add(category);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(category);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        if (dto.Id != id)
            return BadRequest();

        var category = await _dbContext
        .Categories
        .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
            return NotFound();

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(category);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
        .Categories
        .Where(category => category.Id == id)
        .ExecuteDeleteAsync(cancellationToken);

        if (result == 0)
            return NotFound();

        return NoContent();
    }
}