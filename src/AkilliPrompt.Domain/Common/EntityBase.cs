namespace AkilliPrompt.Domain.Common;

public abstract class EntityBase : ICreatedByEntity, IModifiedByEntity
{
    public long Id { get; set; }

    public string? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void ClearDomainEvents() => _domainEvents.Clear();
}
