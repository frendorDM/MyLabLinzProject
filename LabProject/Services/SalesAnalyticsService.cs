using Microsoft.EntityFrameworkCore;
using LabProject.Data;
using LabProject.Models.Customers;
using LabProject.Models.Sales;

namespace LabProject.Services
{
    public class SalesAnalyticsService : ISalesAnalyticsService
    {
        private readonly CustomersContext _customersContext;
        private readonly SalesContext _salesContext;

        public SalesAnalyticsService(CustomersContext customersContext, SalesContext salesContext)
        {
            _customersContext = customersContext;
            _salesContext = salesContext;
        }

        public async Task<Customer?> GetHighestSpendingCustomerAsync()
        {
            var customerSpending = await _salesContext.OrderDetail
                .Include(od => od.Order)
                .GroupBy(od => od.Order.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalSpent = g.Sum(od => od.Amount * od.UnitPrice)
                })
                .OrderByDescending(g => g.TotalSpent)
                .FirstOrDefaultAsync();

            if (customerSpending == null)
                return null;

            return await _customersContext.Customer
                .FirstOrDefaultAsync(c => c.Id == customerSpending.CustomerId);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customersContext.Customer.ToListAsync();
        }

        public async Task<IEnumerable<TopSellingItem>> GetTop5SellingItemsAsync()
        {
            return await _salesContext.OrderDetail
                .Where(od => od.Amount > 0)
                .GroupBy(od => new { od.Sku, od.SkuName })
                .Select(g => new TopSellingItem
                {
                    Sku = g.Key.Sku,
                    SkuName = g.Key.SkuName,
                    TotalQuantity = g.Sum(od => od.Amount)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToListAsync();
        }
    }
} 