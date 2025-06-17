using LabProject.Models.Customers;
using LabProject.Services;

namespace LabProject.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Customer HighestSpendingCustomer { get; set; }
        public IEnumerable<Customer> AllCustomers { get; set; }
        public IEnumerable<TopSellingItem> TopSellingItems { get; set; }
    }
} 