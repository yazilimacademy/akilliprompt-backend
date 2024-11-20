namespace AkilliPrompt.Domain.Common;

public abstract class EntityBase : ICreatedByEntity, IModifiedByEntity
{
    public long Id { get; set; }

    public string? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
