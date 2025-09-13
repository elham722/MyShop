using MyShop.Contracts.DTOs.Customer;
using MyShop.Domain.Entities.Customer;

namespace MyShop.Application.Common.Extensions;

/// <summary>
/// Extension methods for Customer entity
/// </summary>
public static class CustomerExtensions
{
    /// <summary>
    /// Converts Customer entity to CustomerDto
    /// </summary>
    /// <param name="customer">Customer entity</param>
    /// <returns>CustomerDto</returns>
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth,
            Age = customer.Age,
            Email = customer.Email?.Value,
            MobileNumber = customer.MobileNumber?.Value,
            Status = customer.Status.ToString(),
            IsEmailVerified = customer.IsEmailVerified,
            IsPhoneVerified = customer.IsPhoneVerified,
            IsVerified = customer.IsVerified,
            HasCompleteProfile = customer.HasCompleteProfile,
            LastLoginAt = customer.LastLoginAt,
            CreatedAt = customer.CreatedAt,
            CreatedBy = customer.CreatedBy,
            UpdatedAt = customer.UpdatedAt,
            UpdatedBy = customer.UpdatedBy
        };
    }

    /// <summary>
    /// Converts collection of Customer entities to CustomerDto collection
    /// </summary>
    /// <param name="customers">Collection of Customer entities</param>
    /// <returns>Collection of CustomerDto</returns>
    public static IEnumerable<CustomerDto> ToDto(this IEnumerable<Customer> customers)
    {
        return customers.Select(c => c.ToDto());
    }
}