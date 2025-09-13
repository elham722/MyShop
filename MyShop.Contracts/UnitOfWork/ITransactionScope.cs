namespace MyShop.Contracts.UnitOfWork;
public interface ITransactionScope : IDisposable, IAsyncDisposable
{
    IUnitOfWork UnitOfWork { get; }

    string TransactionId { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}