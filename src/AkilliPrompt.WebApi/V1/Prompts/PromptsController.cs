using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Domain.ValueObjects;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Services;
using AkilliPrompt.WebApi.V1.Prompts.Create;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AkilliPrompt.WebApi.V1.Prompts;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class PromptsController : ControllerBase
{
    private readonly string _allPromptsCacheKey = "all-prompts";
    private readonly string _promptKeyCachePrefix = "prompt-";
    private readonly MemoryCacheEntryOptions _cacheOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _dbContext;
    private readonly R2ObjectStorageManager _r2ObjectStorageManager;
    public PromptsController(
        IMemoryCache memoryCache,
        ApplicationDbContext dbContext,
        R2ObjectStorageManager r2ObjectStorageManager)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
        _r2ObjectStorageManager = r2ObjectStorageManager;
        var slidingExpiration = TimeSpan.FromMinutes(10);
        var absoluteExpiration = TimeSpan.FromHours(24);

        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingExpiration)
            .SetAbsoluteExpiration(absoluteExpiration);
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(_allPromptsCacheKey, out List<GetAllPromptsDto> cachedPrompts))
            return Ok(cachedPrompts);

        var prompts = await _dbContext
            .Prompts
            .AsNoTracking()
            .Select(prompt => new GetAllPromptsDto(
                prompt.Id,
                prompt.Title,
                prompt.Description,
                prompt.ImageUrl,
                prompt.IsActive))
            .ToListAsync(cancellationToken);

        _memoryCache.Set(_allPromptsCacheKey, prompts, _cacheOptions);

        return Ok(prompts);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_promptKeyCachePrefix}{id}";

        if (_memoryCache.TryGetValue(cacheKey, out GetByIdPromptDto cachedPrompt))
            return Ok(cachedPrompt);

        var prompt = await _dbContext
            .Prompts
            .AsNoTracking()
            .Include(p => p.PromptCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Placeholders)
            .Where(p => p.Id == id)
            .Select(p => new GetByIdPromptDto(
                p.Id,
                p.Title,
                p.Description,
                p.Content,
                p.ImageUrl,
                p.IsActive,
                p.PromptCategories.Select(pc => new PromptCategoryDto(pc.Category.Id, pc.Category.Name)).ToList(),
                p.Placeholders.Select(ph => new PlaceholderDto(ph.Id, ph.Name)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (prompt is null)
            return NotFound();

        _memoryCache.Set(cacheKey, prompt, _cacheOptions);

        return Ok(prompt);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAsync([FromForm] CreatePromptDto dto, CancellationToken cancellationToken)
    {
        string? imageUrl = null;

        try
        {
            var prompt = Prompt.Create(dto.Title, dto.Description, dto.Content, dto.IsActive);

            // Upload image
            if (dto.Image is not null)
            {
                imageUrl = await _r2ObjectStorageManager.UploadPromptPicAsync(dto.Image, cancellationToken);
                prompt.SetImageUrl(imageUrl);
            }

            _dbContext.Prompts.Add(prompt);

            var promptCategories = dto.CategoryIds
                .Select(categoryId => PromptCategory.Create(prompt.Id, categoryId));

            _dbContext.PromptCategories.AddRange(promptCategories);

            if (dto.PlaceholderNames is not null && dto.PlaceholderNames.Any())
            {
                var placeholders = dto.PlaceholderNames
                    .Select(name => Placeholder.Create(name, prompt.Id));

                _dbContext.Placeholders.AddRange(placeholders);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            InvalidateCache();

            return Ok(ResponseDto<Guid>.Success(prompt.Id, MessageHelper.GetApiSuccessCreatedMessage("Prompt")));
        }
        catch (Exception ex)
        {
            if (imageUrl is not null)
                await _r2ObjectStorageManager.RemovePromptPicAsync(imageUrl, cancellationToken);

            throw;
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdatePromptDto dto, CancellationToken cancellationToken)
    {

        if (dto.Id != id)
            return BadRequest();

        var prompt = await _dbContext
            .Prompts
            .Include(p => p.PromptCategories)
            .Include(p => p.Placeholders)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (prompt is null)
            return NotFound();

        prompt.Update(dto.Title, dto.Description, dto.Content, dto.IsActive);

        // Update categories
        prompt.PromptCategories.Clear();
        var categories = await _dbContext.Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        foreach (var category in categories)
        {
            prompt.PromptCategories.Add(new PromptCategory { Category = category });
        }

        // Update placeholders
        prompt.Placeholders.Clear();
        foreach (var placeholderName in dto.PlaceholderNames)
        {
            prompt.Placeholders.Add(new Placeholder { Name = placeholderName });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        InvalidateCache(id);

        return Ok(ResponseDto<Guid>.Success(MessageHelper.GetApiSuccessUpdatedMessage("Prompt")));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .Prompts
            .Where(prompt => prompt.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (result == 0)
            return NotFound();

        InvalidateCache(id);

        return Ok(ResponseDto<long>.Success(MessageHelper.GetApiSuccessDeletedMessage("Prompt")));
    }

    private void InvalidateCache(Guid? promptId = null)
    {
        _memoryCache.Remove(_allPromptsCacheKey);

        if (promptId.HasValue)
            _memoryCache.Remove($"{_promptKeyCachePrefix}{promptId}");
    }
}