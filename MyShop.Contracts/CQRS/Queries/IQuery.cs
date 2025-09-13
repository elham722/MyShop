namespace MyShop.Contracts.CQRS.Queries;
public interface IQuery<out TResponse> : IRequest<TResponse> { }