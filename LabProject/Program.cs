using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LabProject.Data;
using LabProject.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<SalesContext>(options =>
        options.UseInMemoryDatabase("SalesDb"));
    builder.Services.AddDbContext<CustomersContext>(options =>
        options.UseInMemoryDatabase("CustomersDb"));
}
else
{
    builder.Services.AddDbContext<SalesContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SalesContext") ?? throw new InvalidOperationException("Connection string 'SalesContext' not found.")));
    builder.Services.AddDbContext<CustomersContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersContext") ?? throw new InvalidOperationException("Connection string 'CustomersContext' not found.")));
}

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISalesAnalyticsService, SalesAnalyticsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await DataSeeder.SeedData(app.Services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
