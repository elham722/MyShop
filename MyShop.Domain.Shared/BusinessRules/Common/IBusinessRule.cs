namespace MyShop.Domain.Shared.BusinessRules.Common;
public interface IBusinessRule
{
    bool IsBroken();

    string Message { get; }

    Task<bool> IsBrokenAsync();
}