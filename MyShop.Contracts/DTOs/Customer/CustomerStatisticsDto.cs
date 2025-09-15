namespace MyShop.Contracts.DTOs.Customer;

public class CustomerStatisticsDto
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int VerifiedCustomers { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public double AverageAge { get; set; }
    public Dictionary<string, int> CustomersByStatus { get; set; } = new();
    public Dictionary<string, int> CustomersByMonth { get; set; } = new();
}