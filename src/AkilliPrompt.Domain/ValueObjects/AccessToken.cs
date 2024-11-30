namespace AkilliPrompt.Domain.ValueObjects;

public sealed record AccessToken
{
    public string Value { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public bool IsExpired() => ExpiresOnUtc < DateTime.UtcNow;

    public AccessToken(string value, DateTime expiresOnUtc)
    {
        Value = value;
        ExpiresOnUtc = expiresOnUtc;
    }
}
