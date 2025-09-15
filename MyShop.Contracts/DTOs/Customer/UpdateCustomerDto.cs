namespace MyShop.Contracts.DTOs.Customer;

public class UpdateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
}