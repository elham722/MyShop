using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Services.Application;
public interface IApplicationService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Service Access

    ICommandService<T, TId> CommandService { get; }

    IQueryService<T, TId> QueryService { get; }

    #endregion

    #region Business Operations

    Task<TResult> ExecuteBusinessOperationAsync<TResult>(Func<ICommandService<T, TId>, IQueryService<T, TId>, Task<TResult>> operation, CancellationToken cancellationToken = default);

    Task ExecuteBusinessOperationAsync(Func<ICommandService<T, TId>, IQueryService<T, TId>, Task> operation, CancellationToken cancellationToken = default);

    #endregion

    #region Transaction Management

    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<ICommandService<T, TId>, IQueryService<T, TId>, Task<TResult>> operation, CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(Func<ICommandService<T, TId>, IQueryService<T, TId>, Task> operation, CancellationToken cancellationToken = default);

    #endregion
}