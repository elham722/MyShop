namespace MyShop.Contracts.DTOs.Customer;

/// <summary>
/// Customer data transfer object
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsVerified { get; set; }
    public bool HasCompleteProfile { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// Create customer request DTO
/// </summary>
public class CreateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
}

/// <summary>
/// Update customer request DTO
/// </summary>
public class UpdateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
}

/// <summary>
/// Customer list query parameters
/// </summary>
public class GetCustomersQueryParams
{
    public PaginationParams Pagination { get; set; } = PaginationParams.Default;
    public SortDtoCollection Sorting { get; set; } = new();
    public FilterDtoCollection Filtering { get; set; } = new();
    public string? Search { get; set; }
}