namespace AkilliPrompt.Domain.ValueObjects;

public sealed record ValidationError
{
    public string PropertyName { get; init; }
    public string ErrorMessage { get; init; }

    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}
