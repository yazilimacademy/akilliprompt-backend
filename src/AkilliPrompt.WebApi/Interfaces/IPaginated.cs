namespace AkilliPrompt.WebApi.Interfaces;

public interface IPaginated
{
    int PageNumber { get; }
    int PageSize { get; }
}
