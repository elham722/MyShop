namespace MyShop.Domain.Shared.BusinessRules.Common;

public abstract class BaseBusinessRule : IBusinessRule
{
    public abstract bool IsBroken();

    public abstract string Message { get; }

    public virtual Task<bool> IsBrokenAsync()
    {
        return Task.FromResult(IsBroken());
    }

    public bool IsSatisfied() => !IsBroken();

    public virtual async Task<bool> IsSatisfiedAsync() => !await IsBrokenAsync();

    public virtual void Validate()
    {
        ThrowIfBroken();
    }

    public virtual async Task ValidateAsync()
    {
        await ThrowIfBrokenAsync();
    }

    protected void ThrowIfBroken()
    {
        if (IsBroken())
            throw new BusinessRuleViolationException(Message);
    }

    protected async Task ThrowIfBrokenAsync()
    {
        if (await IsBrokenAsync())
            throw new BusinessRuleViolationException(Message);
    }
}