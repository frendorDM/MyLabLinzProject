using LabProject.Models.Customers;
using LabProject.Models.Sales;

namespace LabProject.Services
{
    public interface ISalesAnalyticsService
    {
        Task<Customer> GetHighestSpendingCustomerAsync();
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<TopSellingItem>> GetTop5SellingItemsAsync();
    }

    public class TopSellingItem
    {
        public int Sku { get; set; }
        public string SkuName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }
} 