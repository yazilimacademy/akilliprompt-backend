namespace AkilliPrompt.Domain.Settings;

public sealed record GoogleAuthSettings
{
    public string ClientId { get; init; }

    public GoogleAuthSettings(string clientId)
    {
        ClientId = clientId;
    }

    public GoogleAuthSettings()
    {

    }
};