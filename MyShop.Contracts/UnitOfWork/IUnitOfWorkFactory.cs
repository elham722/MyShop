namespace MyShop.Contracts.UnitOfWork;
public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();

    Task<ITransactionScope> CreateTransactionScopeAsync(CancellationToken cancellationToken = default);

    Task<ITransactionScope> CreateTransactionScopeAsync(string isolationLevel, CancellationToken cancellationToken = default);
}