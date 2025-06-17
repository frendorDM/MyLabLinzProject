using LabProject.Models.Customers;
using LabProject.Models.Sales;
using Microsoft.EntityFrameworkCore;

namespace LabProject.Data
{
  public static class DataSeeder
  {
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
      using var scope = serviceProvider.CreateScope();
      var customersContext = scope.ServiceProvider.GetRequiredService<CustomersContext>();
      var salesContext = scope.ServiceProvider.GetRequiredService<SalesContext>();

      if (customersContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
      {
        await customersContext.Database.MigrateAsync();
        await salesContext.Database.MigrateAsync();
      }
      else
      {
        await customersContext.Database.EnsureCreatedAsync();
        await salesContext.Database.EnsureCreatedAsync();
      }

      if (!await customersContext.Customer.AnyAsync())
      {
        var customers = new List<Customer>
                {
                    new Customer
                    {
                        Name = "Denis",
                        Surname = "Martianov",
                        Lastname = "Alekseevich",
                        DateOfBirthday = new DateTime(1997, 6, 15),
                        Email = "denis.martianovHelloTeam@example.com",
                        CountryOfResidence = "Russia"
                    },
                    new Customer
                    {
                        Name = "Anna",
                        Surname = "Smirnova",
                        Lastname = "Igorevna",
                        DateOfBirthday = new DateTime(1987, 3, 22),
                        Email = "anna.smirnova@example.com",
                        CountryOfResidence = "Ukraine"
                    },
                    new Customer
                    {
                        Name = "Fernando",
                        Surname = "Torres",
                        Lastname = "Sanz",
                        DateOfBirthday = new DateTime(1984, 3, 20),
                        Email = "fernando.torres@example.com",
                        CountryOfResidence = "Spain"
                    },
                    new Customer
                    {
                        Name = "Julio",
                        Surname = "Iglesias",
                        Lastname = "de la Cueva",
                        DateOfBirthday = new DateTime(1943, 9, 23),
                        Email = "julio.iglesias@example.com",
                        CountryOfResidence = "Spain"
                    },
                    new Customer
                    {
                        Name = "Enrique",
                        Surname = "Iglesias",
                        Lastname = "Preysler",
                        DateOfBirthday = new DateTime(1975, 5, 8),
                        Email = "enrique.iglesias@example.com",
                        CountryOfResidence = "Spain"
                    },
                    new Customer
                    {
                        Name = "Andres",
                        Surname = "Iniesta",
                        Lastname = "Lujan",
                        DateOfBirthday = new DateTime(1984, 5, 11),
                        Email = "andres.iniesta@example.com",
                        CountryOfResidence = "Spain"
                    },
                    new Customer
                    {
                        Name = "Jane",
                        Surname = "Smith",
                        Lastname = "Johnson",
                        DateOfBirthday = new DateTime(1985, 5, 15),
                        Email = "jane.smith@example.com",
                        CountryOfResidence = "Canada"
                    },
                    new Customer
                    {
                        Name = "Michael",
                        Surname = "Johnson",
                        Lastname = "Brown",
                        DateOfBirthday = new DateTime(1990, 8, 20),
                        Email = "michael.johnson@example.com",
                        CountryOfResidence = "UK"
                    },
                    new Customer
                    {
                        Name = "Sergio",
                        Surname = "Ramos",
                        Lastname = "García",
                        DateOfBirthday = new DateTime(1986, 3, 30),
                        Email = "sergio.ramos@example.com",
                        CountryOfResidence = "Spain"
                    },
                    new Customer
                    {
                        Name = "Alejandro",
                        Surname = "Sanz",
                        Lastname = "Pizarro",
                        DateOfBirthday = new DateTime(1968, 12, 18),
                        Email = "alejandro.sanz@example.com",
                        CountryOfResidence = "Spain"
                    }
        };

        await customersContext.Customer.AddRangeAsync(customers);
        await customersContext.SaveChangesAsync();

        var products = new List<(int Sku, string Name, double Price)>
                {
                    (1001, "Laptop", 999.99),
                    (1002, "Smartphone", 699.99),
                    (1003, "Headphones", 199.99),
                    (1004, "Tablet", 499.99),
                    (1005, "Smartwatch", 299.99),
                    (1006, "Camera", 799.99),
                    (1007, "Speaker", 149.99)
                };

        var random = new Random(42);
        var orders = new List<Order>();

        foreach (var customer in customers)
        {          
          var orderCount = random.Next(2, 5);
          for (int i = 0; i < orderCount; i++)
          {
            var order = new Order
            {
              CustomerId = customer.Id,
              OrderDate = DateTime.Now.AddDays(-random.Next(1, 365))
            };

            var itemCount = random.Next(1, 6);
            var orderDetails = new List<OrderDetail>();
            
            for (int j = 0; j < itemCount; j++)
            {
              var product = products[random.Next(products.Count)];
              var amount = random.Next(1, 4);
              
              orderDetails.Add(new OrderDetail
              {
                OrderId = order.Id,
                Sku = product.Sku,
                SkuName = product.Name,
                Amount = amount,
                UnitPrice = product.Price
              });
            }

            orders.Add(order);
            await salesContext.Order.AddAsync(order);
            await salesContext.SaveChangesAsync();

            foreach (var detail in orderDetails)
            {
              detail.OrderId = order.Id;
            }
            await salesContext.OrderDetail.AddRangeAsync(orderDetails);
          }
        }

        await salesContext.SaveChangesAsync();
      }
    }
  }
}