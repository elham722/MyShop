namespace MyShop.Domain.Exceptions.Persistence;
public class ConcurrencyException : DomainException
{
    public new const string ErrorCode = DomainErrorCodes.ConcurrencyConflict;

    public int ExpectedVersion { get; }

    public int ActualVersion { get; }

    public Guid AggregateId { get; }

    public ConcurrencyException(Guid aggregateId, int expectedVersion, int actualVersion)
        : base($"Concurrency conflict for aggregate {aggregateId}. Expected version {expectedVersion}, but actual version is {actualVersion}.", ErrorCode)
    {
        AggregateId = aggregateId;
        ExpectedVersion = expectedVersion;
        ActualVersion = actualVersion;
    }

    public ConcurrencyException(string message, Guid aggregateId, int expectedVersion, int actualVersion)
        : base(message, ErrorCode)
    {
        AggregateId = aggregateId;
        ExpectedVersion = expectedVersion;
        ActualVersion = actualVersion;
    }

    public ConcurrencyException(string message, Exception innerException, Guid aggregateId, int expectedVersion, int actualVersion)
        : base(message, ErrorCode, innerException)
    {
        AggregateId = aggregateId;
        ExpectedVersion = expectedVersion;
        ActualVersion = actualVersion;
    }

    public override string ToString()
    {
        return $"ConcurrencyException: {Message} (ErrorCode: {ErrorCode}, AggregateId: {AggregateId}, ExpectedVersion: {ExpectedVersion}, ActualVersion: {ActualVersion})";
    }
}
