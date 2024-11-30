namespace AkilliPrompt.Domain.ValueObjects;

public sealed record RefreshToken
{
    public string Value { get; set; }
    public DateTime ExpiresOnUtc { get; set; }

    public bool IsExpired() => ExpiresOnUtc < DateTime.UtcNow;

    public RefreshToken(string value, DateTime expiresOnUtc)
    {
        Value = value;
        ExpiresOnUtc = expiresOnUtc;
    }
}
