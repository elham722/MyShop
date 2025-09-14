using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.Factories;
public interface IRepositoryFactory
{
    #region Basic Repository Creation

    ICommandRepository<T, TId> CreateCommandRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IQueryRepository<T, TId> CreateQueryRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Bulk Repository Creation

    IBulkCommandRepository<T, TId> CreateBulkCommandRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    ICombinedCommandRepository<T, TId> CreateCombinedCommandRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Repository Validation

    bool IsBulkOperationsSupported<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    string[] GetSupportedBulkOperations<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion
}
