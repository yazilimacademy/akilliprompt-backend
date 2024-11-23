namespace AkilliPrompt.Domain.Settings;

public sealed record CloudflareR2Settings
{
    public string ServiceUrl { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string PromptPicsBucketName { get; set; }
    public string UserPicsBucketName { get; set; }
}
