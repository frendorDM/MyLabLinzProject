using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LabProject.Models.Customers;

namespace LabProject.Data
{
    public class CustomersContext : DbContext
    {
        public CustomersContext (DbContextOptions<CustomersContext> options)
            : base(options)
        {
        }

        public DbSet<LabProject.Models.Customers.Customer> Customer { get; set; } = default!;
    }
}
