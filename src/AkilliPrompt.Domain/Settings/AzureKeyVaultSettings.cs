namespace AkilliPrompt.Domain.Settings;

public sealed record AzureKeyVaultSettings
{
    public string Uri { get; init; }
    public string TenantId { get; init; }
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
}
