using MyShop.Application.Common.Interfaces;
using MyShop.Domain.Entities.Customer;
using MyShop.Domain.ValueObjects.Customer;

namespace MyShop.Application.Common.Repositories;

/// <summary>
/// Mock implementation of ICustomerRepository for testing purposes
/// </summary>
public class MockCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private readonly Dictionary<Guid, Customer> _customerDict = new();

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _customerDict.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }

    public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Customer>>(_customers.ToList());
    }

    public Task<IQueryable<Customer>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_customers.AsQueryable());
    }

    public Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _customers.Add(customer);
        _customerDict[customer.Id] = customer;
        return Task.FromResult(customer);
    }

    public Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var index = _customers.FindIndex(c => c.Id == customer.Id);
        if (index >= 0)
        {
            _customers[index] = customer;
            _customerDict[customer.Id] = customer;
        }
        return Task.FromResult(customer);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _customers.Remove(customer);
            _customerDict.Remove(id);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_customerDict.ContainsKey(id));
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var exists = _customers.Any(c => c.Email != null && c.Email.Value.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }

    public Task<bool> PhoneExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var exists = _customers.Any(c => c.MobileNumber != null && c.MobileNumber.Value.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }

    // Helper method to seed test data
    public void SeedTestData()
    {
        var customer1 = new Customer("علی", "احمدی", "System");
        customer1.SetEmail(new Email("ali@example.com"));
        customer1.SetMobileNumber(new PhoneNumber("09123456789"));
        customer1.SetDateOfBirth(new DateTime(1990, 5, 15));
        _customers.Add(customer1);
        _customerDict[customer1.Id] = customer1;

        var customer2 = new Customer("فاطمه", "رضایی", "System");
        customer2.SetEmail(new Email("fateme@example.com"));
        customer2.SetMobileNumber(new PhoneNumber("09129876543"));
        customer2.SetDateOfBirth(new DateTime(1985, 8, 20));
        _customers.Add(customer2);
        _customerDict[customer2.Id] = customer2;

        var customer3 = new Customer("محمد", "کریمی", "System");
        customer3.SetEmail(new Email("mohammad@example.com"));
        customer3.SetMobileNumber(new PhoneNumber("09187654321"));
        customer3.SetDateOfBirth(new DateTime(1992, 12, 10));
        _customers.Add(customer3);
        _customerDict[customer3.Id] = customer3;
    }
}