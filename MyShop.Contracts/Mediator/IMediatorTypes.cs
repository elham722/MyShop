namespace MyShop.Contracts.Mediator;
public interface IRequest<out TResponse> { }

public interface IRequest : IRequest<Unit> { }

public interface INotification { }

public struct Unit
{
    public static readonly Unit Value = new();
}
