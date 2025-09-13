namespace MyShop.Domain.BusinessRules.Common;
public interface IBusinessRule
{
    bool IsBroken();

    string Message { get; }

    Task<bool> IsBrokenAsync();
}