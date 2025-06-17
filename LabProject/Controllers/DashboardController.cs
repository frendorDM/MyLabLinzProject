using Microsoft.AspNetCore.Mvc;
using LabProject.Models.ViewModels;
using LabProject.Services;

namespace LabProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISalesAnalyticsService _salesAnalyticsService;

        public DashboardController(ISalesAnalyticsService salesAnalyticsService)
        {
            _salesAnalyticsService = salesAnalyticsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                HighestSpendingCustomer = await _salesAnalyticsService.GetHighestSpendingCustomerAsync(),
                AllCustomers = await _salesAnalyticsService.GetAllCustomersAsync(),
                TopSellingItems = await _salesAnalyticsService.GetTop5SellingItemsAsync()
            };

            return View(viewModel);
        }
    }
} 