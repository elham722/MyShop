namespace MyShop.Contracts.Extensions;
public static class UnitOfWorkExtensions
{
    #region Save Changes with Events

    public static async Task<int> SaveWithEventsAsync(
        this IUnitOfWork unitOfWork,
        CancellationToken cancellationToken = default)
    {
        var changesCount = await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.DispatchDomainEventsAsync(cancellationToken);
        return changesCount;
    }

    public static async Task<int> SaveWithEventsRollbackAsync(
        this IUnitOfWork unitOfWork,
        CancellationToken cancellationToken = default)
    {
        var changesCount = await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.DispatchDomainEventsWithRollbackAsync(cancellationToken);
        return changesCount;
    }

    #endregion

    #region Execute in Transaction with Events

    public static async Task<TResult> ExecuteInTransactionWithEventsAsync<TResult>(
        this IUnitOfWork unitOfWork,
        Func<Task<TResult>> operation,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await operation();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.DispatchDomainEventsWithRollbackAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public static Task ExecuteInTransactionWithEventsAsync(
        this IUnitOfWork unitOfWork,
        Func<Task> operation,
        CancellationToken cancellationToken = default)
    {
        return unitOfWork.ExecuteInTransactionWithEventsAsync(async () =>
        {
            await operation();
            return Unit.Value;
        }, cancellationToken);
    }

    public static async Task<TResult> ExecuteInTransactionWithEventErrorHandlingAsync<TResult>(
        this IUnitOfWork unitOfWork,
        Func<Task<TResult>> operation,
        Func<Exception, Task> onEventError,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await operation();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.DispatchDomainEventsAsync(onEventError, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    #endregion
}
