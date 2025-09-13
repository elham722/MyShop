namespace MyShop.Contracts.CQRS.Queries.Common;
public interface IQuery<out TResponse> : IRequest<TResponse> { }