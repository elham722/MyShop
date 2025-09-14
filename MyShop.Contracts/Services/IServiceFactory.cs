using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Services;
public interface IServiceFactory
{
    #region Service Creation

    ICommandService<T, TId> CreateCommandService<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IQueryService<T, TId> CreateQueryService<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IApplicationService<T, TId> CreateApplicationService<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Service Validation

    bool IsCommandServiceSupported<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    bool IsQueryServiceSupported<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    bool IsApplicationServiceSupported<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion
}
