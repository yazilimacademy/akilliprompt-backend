using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.V1.Categories.Create;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace AkilliPrompt.WebApi.V1.Categories;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly string _allCategoriesCacheKey = "all-categories";
    private readonly string _categoryKeyCachePrefix = "category-";
    private readonly MemoryCacheEntryOptions _cacheOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _mediator;

    public CategoriesController(
        IMemoryCache memoryCache,
        ApplicationDbContext dbContext,
        ISender mediator)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
        _mediator = mediator;
        var slidingExpiration = TimeSpan.FromMinutes(10);
        var absoluteExpiration = TimeSpan.FromHours(24);

        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingExpiration)
            .SetAbsoluteExpiration(absoluteExpiration);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(_allCategoriesCacheKey, out List<GetAllCategoriesDto> cachedCategories))
            return Ok(cachedCategories);

        var categories = await _dbContext
            .Categories
            .AsNoTracking()
            .Select(category => new GetAllCategoriesDto(category.Id, category.Name))
            .ToListAsync(cancellationToken);

        _memoryCache.Set(_allCategoriesCacheKey, categories, _cacheOptions);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_categoryKeyCachePrefix}{id}";

        if (_memoryCache.TryGetValue(cacheKey, out GetByIdCategoryDto cachedCategory))
            return Ok(cachedCategory);

        var category = await _dbContext
            .Categories
            .AsNoTracking()
            .Select(category => new GetByIdCategoryDto(category.Id, category.Name, category.Description))
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
            return NotFound();

        _memoryCache.Set(cacheKey, category, _cacheOptions);

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        if (dto.Id != id)
            return BadRequest();

        var category = await _dbContext
        .Categories
        .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
            return NotFound();

        category.Update(dto.Name, dto.Description);

        await _dbContext.SaveChangesAsync(cancellationToken);

        InvalidateCache(id);

        return Ok(ResponseDto<long>.Success(MessageHelper.GetApiSuccessUpdatedMessage("Kategori")));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .Categories
            .Where(category => category.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (result == 0)
            return NotFound();

        InvalidateCache(id);

        return Ok(ResponseDto<long>.Success(MessageHelper.GetApiSuccessDeletedMessage("Kategori")));
    }

    private void InvalidateCache(Guid? categoryId = null)
    {
        _memoryCache.Remove(_allCategoriesCacheKey);

        if (categoryId.HasValue)
            _memoryCache.Remove($"{_categoryKeyCachePrefix}{categoryId}");
    }
}