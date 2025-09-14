using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.Command;
public interface ICombinedCommandRepository<T, TId> : ICommandRepository<T, TId>, IBulkCommandRepository<T, TId>
    where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    // This interface combines both ICommandRepository and IBulkCommandRepository
    // No additional methods needed as it inherits from both interfaces
}