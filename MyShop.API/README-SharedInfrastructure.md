# استفاده از زیرساخت مشترک پروژه MyShop

این سند نحوه استفاده صحیح از زیرساخت مشترک تعریف شده در لایه `Contracts` را توضیح می‌دهد.

## قوانین مهم

1. **هیچ‌وقت زیرساخت موجود را دوباره نسازید**
2. **همیشه از کلاس‌های آماده در لایه Contracts استفاده کنید**
3. **تمام سرویس‌ها باید `Result<T>` برگردانند**
4. **تمام کنترلرها باید `ApiResponse<T>` برگردانند**
5. **DTOها و اینترفیس‌ها همیشه در لایه Contracts تعریف شوند**

## اجزای زیرساخت مشترک

### 1. Result Pattern

#### استفاده در لایه Application/Services

```csharp
// موفقیت
return Result<CustomerDto>.Success(customer);

// شکست با یک خطا
return Result<CustomerDto>.Failure("Customer not found");

// شکست با چندین خطا
return Result<CustomerDto>.Failure(new[] { "Error 1", "Error 2" });

// تبدیل خودکار
CustomerDto customer = GetCustomer(); // تبدیل خودکار به Result<CustomerDto>
```

#### تبدیل Result به ApiResponse در کنترلر

```csharp
public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id)
{
    var result = await _customerService.GetCustomerByIdAsync(id);
    return FromResult(result); // استفاده از BaseController
}
```

### 2. ApiResponse Pattern

#### استفاده در کنترلرها

```csharp
// پاسخ موفق
return Success(customer, "Customer retrieved successfully");

// پاسخ خطا
return BadRequest("Invalid input", "VALIDATION_ERROR");

// پاسخ با داده صفحه‌بندی شده
return PagedSuccess(pagedResult, "Customers retrieved successfully");
```

### 3. Pagination

#### استفاده از PaginationParams

```csharp
var pagination = PaginationParams.Create(pageNumber: 1, pageSize: 20);
var queryOptions = QueryOptionsDto.Default.SetPagination(pagination.PageNumber, pagination.PageSize);
```

#### استفاده از PagedResult

```csharp
var pagedResult = PagedResult<CustomerDto>.Create(customers, totalCount, pagination);
return Result<PagedResult<CustomerDto>>.Success(pagedResult);
```

### 4. Filtering

#### استفاده از FilterDto

```csharp
// فیلتر ساده
var filter = FilterDto.Equals("Status", "Active");

// فیلتر با نوع داده
var ageFilter = FilterDto.GreaterThan("Age", "18", "int");

// فیلتر چندگانه
var filters = new List<FilterDto>
{
    FilterDto.Equals("Status", "Active"),
    FilterDto.Contains("FirstName", "John"),
    FilterDto.In("Status", "Active", "Pending")
};

// اضافه کردن به QueryOptions
queryOptions.AddFilter(filter);
```

#### انواع فیلترهای موجود

- `Equals` - برابر
- `NotEquals` - نابرابر
- `Contains` - شامل
- `NotContains` - شامل نیست
- `StartsWith` - شروع با
- `EndsWith` - پایان با
- `GreaterThan` - بزرگتر از
- `LessThan` - کوچکتر از
- `GreaterThanOrEqual` - بزرگتر یا مساوی
- `LessThanOrEqual` - کوچکتر یا مساوی
- `In` - در لیست
- `NotIn` - در لیست نیست
- `IsNull` - null است
- `IsNotNull` - null نیست
- `IsEmpty` - خالی است
- `IsNotEmpty` - خالی نیست

### 5. Sorting

#### استفاده از SortDto

```csharp
// مرتب‌سازی ساده
var sort = SortDto.Ascending("FirstName");

// مرتب‌سازی نزولی
var sort = SortDto.Descending("CreatedAt");

// چندین مرتب‌سازی
var sorts = new List<SortDto>
{
    SortDto.Descending("CreatedAt"),
    SortDto.Ascending("FirstName")
};

// اضافه کردن به QueryOptions
queryOptions.AddSort(sort);
```

### 6. QueryOptions

#### استفاده جامع از QueryOptionsDto

```csharp
var queryOptions = QueryOptionsDto.Default
    .SetPagination(pageNumber: 1, pageSize: 20)
    .SetSearch("John")
    .AddSort(SortDto.Descending("CreatedAt"))
    .AddFilter(FilterDto.Equals("Status", "Active"))
    .AddOption("IncludeInactive", true);
```

### 7. Validation

#### استفاده از ValidationError

```csharp
var validationErrors = new List<ValidationError>
{
    new ValidationError
    {
        ErrorMessage = "First name is required",
        PropertyName = nameof(dto.FirstName),
        Severity = ValidationSeverity.Error
    }
};

return ApiResponse<T>.ValidationError(validationErrors);
```

### 8. Business Rules

#### استفاده از BusinessRuleViolation

```csharp
var violations = new List<BusinessRuleViolation>
{
    new BusinessRuleViolation
    {
        ViolationCode = "CUSTOMER_MIN_AGE",
        ViolationMessage = "Customer must be at least 13 years old",
        RuleName = "MinimumAgeRule",
        Category = "AgeValidation",
        Severity = BusinessRuleSeverity.Error
    }
};

return ApiResponse<T>.BusinessRuleError(violations);
```

## مثال کامل: کنترلر Customer

### کنترلر اصلی

```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomerController : BaseController
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc")
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(pageNumber, pageSize);

        if (!string.IsNullOrWhiteSpace(search))
            queryOptions.SetSearch(search);

        if (!string.IsNullOrWhiteSpace(sortBy))
            queryOptions.AddSort(SortDto.Create(sortBy, sortDirection));

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }
}
```

### سرویس Application

```csharp
public class CustomerService : ICustomerService
{
    public async Task<Result<PagedResult<CustomerDto>>> GetCustomersAsync(QueryOptionsDto options)
    {
        try
        {
            // اعتبارسنجی
            var validationResult = await _validationService.ValidateAsync(options);
            if (!validationResult.IsValid)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // قوانین کسب‌وکار
            var businessRuleResult = await _businessRuleService.ValidateAsync(options);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // اجرای کوئری
            var customers = await _queryService.GetCustomersAsync(options);
            return Result<PagedResult<CustomerDto>>.Success(customers);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to retrieve customers: {ex.Message}");
        }
    }
}
```

### Command Handler

```csharp
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // اعتبارسنجی
            var validationResult = await _validationService.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return Result<CustomerDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // قوانین کسب‌وکار
            var businessRuleResult = await _businessRuleService.ValidateAsync(createDto);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<CustomerDto>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // اجرای دستور
            var customer = await _commandService.CreateCustomerAsync(createDto, request.CreatedBy);
            return Result<CustomerDto>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to create customer: {ex.Message}");
        }
    }
}
```

## نکات مهم

1. **همیشه از `FromResult()` در کنترلرها استفاده کنید**
2. **هیچ‌وقت مستقیماً `Result` را در کنترلر برگردانید**
3. **از `QueryOptionsDto` برای تمام کوئری‌ها استفاده کنید**
4. **فیلترها و مرتب‌سازی را با `FilterDto` و `SortDto` انجام دهید**
5. **خطاها را با `ValidationError` و `BusinessRuleViolation` مدیریت کنید**
6. **از `PagedResult` برای داده‌های صفحه‌بندی شده استفاده کنید**

## فایل‌های ایجاد شده

- `CustomerController.cs` - کنترلر اصلی با استفاده از ApiResponse
- `CustomerService.cs` - سرویس Application با استفاده از Result
- `CreateCustomerCommandHandler.cs` - Command Handler با CQRS
- `GetCustomersQueryHandler.cs` - Query Handler با CQRS
- `CreateCustomerDtoValidator.cs` - اعتبارسنجی با FluentValidation
- `CustomerBusinessRulesService.cs` - قوانین کسب‌وکار
- `CustomerAdvancedController.cs` - مثال‌های پیشرفته فیلترینگ
- `CustomerComprehensiveController.cs` - مثال جامع استفاده از تمام زیرساخت‌ها

این فایل‌ها نشان می‌دهند که چگونه از تمام زیرساخت‌های مشترک به درستی استفاده کنید.