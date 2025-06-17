using Xunit;
using Microsoft.EntityFrameworkCore;
using LabProject.Data;
using LabProject.Models.Customers;
using LabProject.Models.Sales;
using LabProject.Services;

namespace LabProjectTests
{
  public class SalesAnalyticsServiceTests
  {
    private DbContextOptions<CustomersContext> CreateCustomerOptions() =>
    new DbContextOptionsBuilder<CustomersContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

    private DbContextOptions<SalesContext> CreateSalesOptions() =>
        new DbContextOptionsBuilder<SalesContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

    [Fact]
    public async Task GetHighestSpendingCustomer_ReturnsCorrectCustomer()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var customer1 = new Customer { Id = 1, Name = "John", Email = "john@test.com" };
      var customer2 = new Customer { Id = 2, Name = "Jane", Email = "jane@test.com" };

      customersContext.Customer.AddRange(customer1, customer2);
      await customersContext.SaveChangesAsync();

      var order1 = new Order { Id = 1, CustomerId = 1 };
      var order2 = new Order { Id = 2, CustomerId = 2 };

      salesContext.Order.AddRange(order1, order2);
      await salesContext.SaveChangesAsync();

      var orderDetail1 = new OrderDetail { OrderId = 1, Amount = 2, UnitPrice = 10 };
      var orderDetail2 = new OrderDetail { OrderId = 2, Amount = 1, UnitPrice = 5 };

      salesContext.OrderDetail.AddRange(orderDetail1, orderDetail2);
      await salesContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetHighestSpendingCustomerAsync();

      // Assert
      Assert.Equal(1, result.Id);
      Assert.Equal("John", result.Name);
    }

    [Fact]
    public async Task GetTop5SellingItems_ReturnsCorrectItems()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var items = new List<OrderDetail>
            {
                new OrderDetail { Sku = 1, SkuName = "Item1", Amount = 10 },
                new OrderDetail { Sku = 2, SkuName = "Item2", Amount = 20 },
                new OrderDetail { Sku = 3, SkuName = "Item3", Amount = 5 },
                new OrderDetail { Sku = 4, SkuName = "Item4", Amount = 15 },
                new OrderDetail { Sku = 5, SkuName = "Item5", Amount = 25 },
                new OrderDetail { Sku = 6, SkuName = "Item6", Amount = 3 }
            };

      salesContext.OrderDetail.AddRange(items);
      await salesContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetTop5SellingItemsAsync();

      // Assert
      Assert.Equal(5, result.Count());
      Assert.Equal(25, result.First().TotalQuantity);
      Assert.Equal("Item5", result.First().SkuName);
    }

    [Fact]
    public async Task GetHighestSpendingCustomer_ReturnsNull_WhenNoOrders()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetHighestSpendingCustomerAsync();

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task GetTop5SellingItems_ReturnsEmptyList_WhenNoOrderDetails()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetTop5SellingItemsAsync();

      // Assert
      Assert.Empty(result);
    }


    [Fact]
    public async Task GetAllCustomers_ReturnsAllCustomers()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new Customer { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
            };

      customersContext.Customer.AddRange(customers);
      await customersContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetAllCustomersAsync();

      // Assert
      Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetHighestSpendingCustomer_SumsMultipleOrdersCorrectly()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var customer = new Customer { Id = 1, Name = "John Doe", Email = "john@example.com" };
      customersContext.Customer.Add(customer);
      await customersContext.SaveChangesAsync();

      var order1 = new Order { Id = 1, CustomerId = 1 };
      var order2 = new Order { Id = 2, CustomerId = 1 };

      salesContext.Order.AddRange(order1, order2);
      await salesContext.SaveChangesAsync();

      var orderDetail1 = new OrderDetail { OrderId = 1, Amount = 2, UnitPrice = 10 }; // 20$
      var orderDetail2 = new OrderDetail { OrderId = 2, Amount = 1, UnitPrice = 15 }; // 15$

      salesContext.OrderDetail.AddRange(orderDetail1, orderDetail2);
      await salesContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetHighestSpendingCustomerAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(1, result.Id);
      Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task GetHighestSpendingCustomer_HandlesTieBreaker()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var customer1 = new Customer { Id = 1, Name = "John Doe", Email = "john@example.com" };
      var customer2 = new Customer { Id = 2, Name = "Jane Doe", Email = "jane@example.com" };

      customersContext.Customer.AddRange(customer1, customer2);
      await customersContext.SaveChangesAsync();

      var order1 = new Order { Id = 1, CustomerId = 1 };
      var order2 = new Order { Id = 2, CustomerId = 2 };

      salesContext.Order.AddRange(order1, order2);
      await salesContext.SaveChangesAsync();

      var orderDetail1 = new OrderDetail { OrderId = 1, Amount = 2, UnitPrice = 10 }; // 20$
      var orderDetail2 = new OrderDetail { OrderId = 2, Amount = 1, UnitPrice = 20 }; // 20$

      salesContext.OrderDetail.AddRange(orderDetail1, orderDetail2);
      await salesContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetHighestSpendingCustomerAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Contains(result.Id, new List<int> { 1, 2 });
    }

    [Fact]
    public async Task GetTop5SellingItems_ExcludesZeroAmountItems()
    {
      // Arrange
      using var customersContext = new CustomersContext(CreateCustomerOptions());
      using var salesContext = new SalesContext(CreateSalesOptions());

      var items = new List<OrderDetail>
            {
                new OrderDetail { Sku = 1, SkuName = "Item1", Amount = 10 },
                new OrderDetail { Sku = 2, SkuName = "Item2", Amount = 0 },
                new OrderDetail { Sku = 3, SkuName = "Item3", Amount = 5 }
            };

      salesContext.OrderDetail.AddRange(items);
      await salesContext.SaveChangesAsync();

      var service = new SalesAnalyticsService(customersContext, salesContext);

      // Act
      var result = await service.GetTop5SellingItemsAsync();

      // Assert
      Assert.Equal(2, result.Count());
      Assert.DoesNotContain(result, item => item.SkuName == "Item2");
    }
  }
}