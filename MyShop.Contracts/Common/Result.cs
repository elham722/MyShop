using System.Diagnostics;

namespace MyShop.Contracts.Common;

/// <summary>
/// Represents the result of an operation that can either succeed or fail
/// </summary>
[DebuggerDisplay("IsSuccess = {IsSuccess}, Errors = {Errors.Count}")]
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors { get; }
    public string? ErrorCode { get; }
    public DateTime Timestamp { get; }

    protected Result(bool isSuccess, IEnumerable<string>? errors, string? errorCode = null)
    {
        IsSuccess = isSuccess;
        Errors = errors?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
        ErrorCode = errorCode;
        Timestamp = DateTime.UtcNow;
    }

    #region Static Factories

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static Result Success() => new(true, Array.Empty<string>());

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static Result Failure(string error) => new(false, new[] { error });

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);

    /// <summary>
    /// Creates a failed result with error message and error code
    /// </summary>
    public static Result Failure(string error, string errorCode) => new(false, new[] { error }, errorCode);

    /// <summary>
    /// Creates a failed result with multiple errors and error code
    /// </summary>
    public static Result Failure(IEnumerable<string> errors, string errorCode) => new(false, errors, errorCode);

    #endregion

    #region Implicit Conversions

    public static implicit operator Result(string error) => Failure(error);
    public static implicit operator Result(List<string> errors) => Failure(errors);

    #endregion

    #region Functional Helpers

    /// <summary>
    /// Transforms the result using a mapper function if successful
    /// </summary>
    public Result<TOut> Map<TOut>(Func<TOut> mapper)
        => IsSuccess ? Result<TOut>.Success(mapper()) : Result<TOut>.Failure(Errors, ErrorCode);

    /// <summary>
    /// Chains operations that return Results
    /// </summary>
    public Result Bind(Func<Result> binder)
        => IsSuccess ? binder() : this;

    /// <summary>
    /// Executes an action if the result is successful
    /// </summary>
    public Result OnSuccess(Action action)
    {
        if (IsSuccess) action();
        return this;
    }

    /// <summary>
    /// Executes an action if the result is failed
    /// </summary>
    public Result OnFailure(Action<IReadOnlyList<string>> action)
    {
        if (IsFailure) action(Errors);
        return this;
    }

    /// <summary>
    /// Executes an action if the result is failed with specific error code
    /// </summary>
    public Result OnFailure(string errorCode, Action<IReadOnlyList<string>> action)
    {
        if (IsFailure && ErrorCode == errorCode) action(Errors);
        return this;
    }

    #endregion

    #region Matching

    /// <summary>
    /// Executes different functions based on success or failure
    /// </summary>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<string>, TResult> onFailure)
        => IsSuccess ? onSuccess() : onFailure(Errors);

    /// <summary>
    /// Executes different actions based on success or failure
    /// </summary>
    public void Match(Action onSuccess, Action<IReadOnlyList<string>> onFailure)
    {
        if (IsSuccess) onSuccess();
        else onFailure(Errors);
    }

    #endregion

    #region Safe Accessors

    /// <summary>
    /// Gets the first error message or null if successful
    /// </summary>
    public string? GetFirstError() => Errors.FirstOrDefault();

    /// <summary>
    /// Gets all error messages as a single string
    /// </summary>
    public string GetErrorsAsString(string separator = "; ") => string.Join(separator, Errors);

    /// <summary>
    /// Checks if the result has a specific error code
    /// </summary>
    public bool HasErrorCode(string errorCode) => ErrorCode == errorCode;

    #endregion

    #region Overrides

    public override string ToString()
        => IsSuccess ? "Success" : $"Failure: {GetErrorsAsString()}";

    public override bool Equals(object? obj)
    {
        if (obj is not Result other) return false;
        return IsSuccess == other.IsSuccess && 
               Errors.SequenceEqual(other.Errors) && 
               ErrorCode == other.ErrorCode;
    }

    public override int GetHashCode()
        => HashCode.Combine(IsSuccess, Errors, ErrorCode);

    #endregion
}

/// <summary>
/// Represents the result of an operation that can either succeed with a value or fail
/// </summary>
[DebuggerDisplay("IsSuccess = {IsSuccess}, Value = {Value}, Errors = {Errors.Count}")]
public class Result<T> : Result
{
    public T Value { get; }

    // Success constructor
    private Result(T value) : base(true, Array.Empty<string>())
    {
        Value = value;
    }

    // Failure constructor
    private Result(IEnumerable<string> errors, string? errorCode = null) : base(false, errors, errorCode)
    {
        Value = default!;
    }

    #region Static Factories

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static new Result<T> Failure(string error) => new(new[] { error });

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static new Result<T> Failure(IEnumerable<string> errors) => new(errors);

    /// <summary>
    /// Creates a failed result with error message and error code
    /// </summary>
    public static new Result<T> Failure(string error, string errorCode) => new(new[] { error }, errorCode);

    /// <summary>
    /// Creates a failed result with multiple errors and error code
    /// </summary>
    public static new Result<T> Failure(IEnumerable<string> errors, string errorCode) => new(errors, errorCode);

    #endregion

    #region Implicit Conversions

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(string error) => Failure(error);
    public static implicit operator Result<T>(List<string> errors) => Failure(errors);

    #endregion

    #region Functional Helpers

    /// <summary>
    /// Transforms the value using a mapper function if successful
    /// </summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
        => IsSuccess ? Result<TOut>.Success(mapper(Value)) : Result<TOut>.Failure(Errors, ErrorCode);

    /// <summary>
    /// Chains operations that return Results
    /// </summary>
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
        => IsSuccess ? binder(Value) : Result<TOut>.Failure(Errors, ErrorCode);

    /// <summary>
    /// Chains operations that return Results (non-generic)
    /// </summary>
    public Result Bind(Func<T, Result> binder)
        => IsSuccess ? binder(Value) : this;

    /// <summary>
    /// Executes an action with the value if successful
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess) action(Value);
        return this;
    }

    /// <summary>
    /// Executes an action if the result is failed
    /// </summary>
    public new Result<T> OnFailure(Action<IReadOnlyList<string>> action)
    {
        if (IsFailure) action(Errors);
        return this;
    }

    /// <summary>
    /// Executes an action if the result is failed with specific error code
    /// </summary>
    public new Result<T> OnFailure(string errorCode, Action<IReadOnlyList<string>> action)
    {
        if (IsFailure && ErrorCode == errorCode) action(Errors);
        return this;
    }

    /// <summary>
    /// Filters the result based on a predicate
    /// </summary>
    public Result<T> Filter(Func<T, bool> predicate, string errorMessage)
    {
        if (IsFailure) return this;
        return predicate(Value) ? this : Failure(errorMessage);
    }

    /// <summary>
    /// Filters the result based on a predicate with error code
    /// </summary>
    public Result<T> Filter(Func<T, bool> predicate, string errorMessage, string errorCode)
    {
        if (IsFailure) return this;
        return predicate(Value) ? this : Failure(errorMessage, errorCode);
    }

    #endregion

    #region Matching

    /// <summary>
    /// Executes different functions based on success or failure
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<IReadOnlyList<string>, TResult> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Errors);

    /// <summary>
    /// Executes different actions based on success or failure
    /// </summary>
    public void Match(Action<T> onSuccess, Action<IReadOnlyList<string>> onFailure)
    {
        if (IsSuccess) onSuccess(Value);
        else onFailure(Errors);
    }

    #endregion

    #region Safe Accessors

    /// <summary>
    /// Gets the value or throws an exception if failed
    /// </summary>
    public T GetValueOrThrow()
    {
        if (IsFailure)
            throw new InvalidOperationException($"Cannot get value from failed result. Errors: {GetErrorsAsString()}");

        return Value;
    }

    /// <summary>
    /// Gets the value or returns a default value if failed
    /// </summary>
    public T GetValueOrDefault(T defaultValue = default!)
        => IsSuccess ? Value : defaultValue;

    /// <summary>
    /// Gets the value or returns the result of a factory function if failed
    /// </summary>
    public T GetValueOrElse(Func<T> defaultValueFactory)
        => IsSuccess ? Value : defaultValueFactory();

    /// <summary>
    /// Gets the value or returns the result of a factory function with errors if failed
    /// </summary>
    public T GetValueOrElse(Func<IReadOnlyList<string>, T> defaultValueFactory)
        => IsSuccess ? Value : defaultValueFactory(Errors);

    #endregion

    #region Async Helpers

    /// <summary>
    /// Maps the value asynchronously if successful
    /// </summary>
    public async Task<Result<TOut>> MapAsync<TOut>(Func<T, Task<TOut>> mapper)
        => IsSuccess ? Result<TOut>.Success(await mapper(Value)) : Result<TOut>.Failure(Errors, ErrorCode);

    /// <summary>
    /// Binds the result asynchronously if successful
    /// </summary>
    public async Task<Result<TOut>> BindAsync<TOut>(Func<T, Task<Result<TOut>>> binder)
        => IsSuccess ? await binder(Value) : Result<TOut>.Failure(Errors, ErrorCode);

    /// <summary>
    /// Executes an action asynchronously with the value if successful
    /// </summary>
    public async Task<Result<T>> OnSuccessAsync(Func<T, Task> action)
    {
        if (IsSuccess) await action(Value);
        return this;
    }

    #endregion

    #region Overrides

    public override string ToString()
        => IsSuccess ? $"Success: {Value}" : $"Failure: {GetErrorsAsString()}";

    public override bool Equals(object? obj)
    {
        if (obj is not Result<T> other) return false;
        return IsSuccess == other.IsSuccess && 
               EqualityComparer<T>.Default.Equals(Value, other.Value) &&
               Errors.SequenceEqual(other.Errors) && 
               ErrorCode == other.ErrorCode;
    }

    public override int GetHashCode()
        => HashCode.Combine(IsSuccess, Value, Errors, ErrorCode);

    #endregion
}

/// <summary>
/// Extension methods for Result types
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a Task<Result<T>> to Result<T>
    /// </summary>
    public static async Task<Result<T>> UnwrapAsync<T>(this Task<Result<T>> task)
        => await task;

    /// <summary>
    /// Converts a Task<Result> to Result
    /// </summary>
    public static async Task<Result> UnwrapAsync(this Task<Result> task)
        => await task;

    /// <summary>
    /// Combines multiple Results into a single Result
    /// </summary>
    public static Result Combine(this IEnumerable<Result> results)
    {
        var errors = results.Where(r => r.IsFailure).SelectMany(r => r.Errors).ToList();
        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    /// <summary>
    /// Combines multiple Results into a single Result with values
    /// </summary>
    public static Result<IEnumerable<T>> Combine<T>(this IEnumerable<Result<T>> results)
    {
        var errors = results.Where(r => r.IsFailure).SelectMany(r => r.Errors).ToList();
        if (errors.Any()) return Result<IEnumerable<T>>.Failure(errors);
        
        var values = results.Where(r => r.IsSuccess).Select(r => r.Value);
        return Result<IEnumerable<T>>.Success(values);
    }

    /// <summary>
    /// Converts a Result<T> to Result if the value matches a predicate
    /// </summary>
    public static Result ToResult<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
        => result.IsSuccess && predicate(result.Value) ? Result.Success() : Result.Failure(errorMessage);

    /// <summary>
    /// Converts a nullable value to Result
    /// </summary>
    public static Result<T> ToResult<T>(this T? value, string errorMessage = "Value is null")
        => value is not null ? Result<T>.Success(value) : Result<T>.Failure(errorMessage);

    /// <summary>
    /// Converts a nullable value to Result with error code
    /// </summary>
    public static Result<T> ToResult<T>(this T? value, string errorMessage, string errorCode)
        => value is not null ? Result<T>.Success(value) : Result<T>.Failure(errorMessage, errorCode);
}
