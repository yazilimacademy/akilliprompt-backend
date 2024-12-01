namespace AkilliPrompt.Domain.ValueObjects;

public sealed record ValidationError
{
    public string PropertyName { get; init; }
    public IEnumerable<string> ErrorMessages { get; init; }

    public ValidationError(string propertyName, IEnumerable<string> errorMessages)
    {
        PropertyName = propertyName;
        ErrorMessages = errorMessages;
    }

    public ValidationError(string propertyName, string errorMessage)
        : this(propertyName, new List<string> { errorMessage })
    {
    }
}
