using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AkilliPrompt.WebApi.Models;

public sealed record PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public int PageSize { get; set; }

    public PaginatedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();

        TotalCount = totalCount;

        PageNumber = pageNumber;

        PageSize = pageSize;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    [JsonConstructor]
    public PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;
        Items = items;
        PageSize = pageSize;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();

        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();

        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<T> Create(List<T> source, int totalCount, int pageNumber, int pageSize)
    {
        return new PaginatedList<T>(source, totalCount, pageNumber, pageSize);
    }
}
