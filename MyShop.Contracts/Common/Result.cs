namespace MyShop.Contracts.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors { get; }

    protected Result(bool isSuccess, IEnumerable<string>? errors)
    {
        IsSuccess = isSuccess;
        Errors = errors?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
    }

    // ----- Static factories -----
    public static Result Success() => new(true, Array.Empty<string>());
    public static Result Failure(string error) => new(false, new[] { error });
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);

    // ----- Implicit conversions -----
    public static implicit operator Result(string error) => Failure(error);
    public static implicit operator Result(List<string> errors) => Failure(errors);
}

public class Result<T> : Result
{
    public T Value { get; }

    // Success constructor
    private Result(T value) : base(true, Array.Empty<string>())
    {
        Value = value;
    }

    // Failure constructor
    private Result(IEnumerable<string> errors) : base(false, errors)
    {
        Value = default!;
    }

    // ----- Static factories -----
    public static Result<T> Success(T value) => new(value);
    public static new Result<T> Failure(string error) => new(new[] { error });
    public static new Result<T> Failure(IEnumerable<string> errors) => new(errors);

    // ----- Implicit conversions -----
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(string error) => Failure(error);
    public static implicit operator Result<T>(List<string> errors) => Failure(errors);

    // ----- Functional helpers -----
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
        => IsSuccess ? Result<TOut>.Success(mapper(Value)) : Result<TOut>.Failure(Errors);

    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
        => IsSuccess ? binder(Value) : Result<TOut>.Failure(Errors);

    // ----- Matching -----
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<IReadOnlyList<string>, TResult> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Errors);

    public void Match(Action<T> onSuccess, Action<IReadOnlyList<string>> onFailure)
    {
        if (IsSuccess) onSuccess(Value);
        else onFailure(Errors);
    }

    // ----- Safe accessors -----
    public T GetValueOrThrow()
    {
        if (IsFailure)
            throw new InvalidOperationException($"Cannot get value from failed result. Errors: {string.Join(", ", Errors)}");

        return Value;
    }

    public T GetValueOrDefault(T defaultValue = default!)
        => IsSuccess ? Value : defaultValue;
}
