namespace MyShop.Domain.Aggregates.Common;
public abstract class BaseAggregateRoot<TId> : AuditableEntity<TId>, IAggregateRoot
        where TId : IEquatable<TId>
{
    public int Version { get; private set; }

    private readonly List<BaseDomainEvent> _historicalEvents = new();

    public IReadOnlyCollection<BaseDomainEvent> CommittedEvents => _historicalEvents.AsReadOnly();

    #region Constructors

    protected BaseAggregateRoot() : base()
    {
        Version = 0;
    }

    protected BaseAggregateRoot(TId id) : base(id)
    {
        Version = 0;
    }

    #endregion

    #region Version Management

    protected void IncrementVersion() => Version++;

    public void SetVersion(int version)
    {
        if (version < 0)
            throw new CustomValidationException("Version cannot be negative");
        Version = version;
    }

    public void IncrementVersionInternal() => Version++;

    #endregion

    #region Aggregate Event Management

    public void AddAggregateEvent(BaseDomainEvent @event)
    {
        Guard.AgainstNull(@event, nameof(@event));
        _historicalEvents.Add(@event);
        AddDomainEvent(@event);
    }

    public void AddAggregateEventPublic(BaseDomainEvent @event)
    {
        AddAggregateEvent(@event);
    }

    internal void ClearAggregateEventsInternal()
    {
        _historicalEvents.Clear();
        ClearDomainEvents();
    }

    public void ClearAggregateEvents()
    {
        _historicalEvents.Clear();
        ClearDomainEvents();
    }

    public void ClearPendingDomainEvents()
    {
        ClearDomainEvents();
    }

    public void ClearAggregateEventsOnly()
    {
        _historicalEvents.Clear();
    }

    public bool HasAggregateEvents => _historicalEvents.Count > 0;

    public bool HasNewEvents => HasDomainEvents;

    public IReadOnlyCollection<BaseDomainEvent> GetUncommittedDomainEvents()
    {
        return DomainEvents;
    }

    public IReadOnlyCollection<BaseDomainEvent> GetAllEvents() => _historicalEvents.ToList().AsReadOnly();


    #endregion

    #region Helper Methods for Property Updates with Events

    public void UpdateWithEvent(Action updateAction, BaseDomainEvent domainEvent)
    {
        updateAction?.Invoke();
        IncrementVersion();
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(domainEvent);
    }

    protected void UpdateWithEvent(Action updateAction, BaseDomainEvent domainEvent, Action additionalAction)
    {
        updateAction?.Invoke();
        IncrementVersion();
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(domainEvent);
        additionalAction?.Invoke();
    }

    protected void UpdateWithEvent<T>(Action updateAction, BaseDomainEvent domainEvent, Action<T> additionalAction, T oldValue)
    {
        updateAction?.Invoke();
        IncrementVersion();
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(domainEvent);
        additionalAction?.Invoke(oldValue);
    }

    protected void UpdateWithEvent<T>(Action updateAction, BaseDomainEvent domainEvent, Action<T, T> additionalAction, T oldValue, T newValue)
    {
        updateAction?.Invoke();
        IncrementVersion();
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(domainEvent);
        additionalAction?.Invoke(oldValue, newValue);
    }

    protected void UpdateWithEvent<T>(Action updateAction, BaseDomainEvent domainEvent, Func<T, bool> condition, Action<T> additionalAction, T oldValue)
    {
        updateAction?.Invoke();
        IncrementVersion();
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(domainEvent);

        if (condition?.Invoke(oldValue) == true)
        {
            additionalAction?.Invoke(oldValue);
        }
    }

    #endregion

    #region Validation / Invariants

    protected virtual void ValidateState() { }

    protected void EnsureInvariants() => ValidateState();

    public void EnsureValidState() => EnsureInvariants();

    #endregion

    #region Lifecycle Hooks (optional override in child aggregates)

    protected override void OnCreated()
    {
        base.OnCreated();
        // Version is already incremented in MarkAsCreatedInternal
    }

    protected override void OnUpdated()
    {
        base.OnUpdated();
        // Version is already incremented in MarkAsUpdatedInternal
    }

    protected override void OnDeleted()
    {
        base.OnDeleted();
        // Version is already incremented in MarkAsDeletedInternal
    }

    public override void MarkAsCreated(string createdBy)
    {
        MarkAsCreated(createdBy, DateTime.UtcNow);
    }

    public override void MarkAsCreated(string createdBy, DateTime createdAt)
    {
        base.MarkAsCreated(createdBy, createdAt);
        OnCreated();
    }

    public override void MarkAsUpdated(string updatedBy)
    {
        MarkAsUpdated(updatedBy, DateTime.UtcNow);
    }

    public override void MarkAsUpdated(string updatedBy, DateTime updatedAt)
    {
        base.MarkAsUpdated(updatedBy, updatedAt);
        OnUpdated();
    }

    public override void MarkAsDeleted(string deletedBy)
    {
        MarkAsDeleted(deletedBy, DateTime.UtcNow);
    }

    public override void MarkAsDeleted(string deletedBy, DateTime deletedAt)
    {
        base.MarkAsDeleted(deletedBy, deletedAt);
        OnDeleted();
    }


    public void MarkAsCreatedInternal()
    {
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        IncrementVersion();
        OnCreated();
    }

    public void MarkAsUpdatedInternal()
    {
        UpdatedBy = "System";
        UpdatedAt = DateTime.UtcNow;
        IncrementVersion();
        OnUpdated();
    }

    public void MarkAsDeletedInternal()
    {
        DeletedBy = "System";
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        IncrementVersion();
        OnDeleted();
    }


    public void RestoreFromPersistence(int version, IEnumerable<BaseDomainEvent>? events = null)
    {
        SetVersion(version);
        if (events != null)
        {
            foreach (var @event in events)
            {
                _historicalEvents.Add(@event);
                // Note: We don't add to domain events here to avoid double publishing
                // Domain events are only for new events that need to be published
            }
        }
    }

    public void ReplayEvents(IEnumerable<BaseDomainEvent> events)
    {
        if (events == null) return;

        foreach (var @event in events)
        {
            _historicalEvents.Add(@event);
            AddDomainEvent(@event);
        }
    }

    public void RestoreFromPersistenceWithReplay(int version, IEnumerable<BaseDomainEvent>? events = null, bool replayEvents = false)
    {
        SetVersion(version);
        if (events != null)
        {
            // Clear existing domain events if replaying
            if (replayEvents)
            {
                ClearDomainEvents();
            }

            foreach (var @event in events)
            {
                _historicalEvents.Add(@event);
                if (replayEvents)
                {
                    AddDomainEvent(@event);
                }
            }
        }
    }

    #endregion
}

