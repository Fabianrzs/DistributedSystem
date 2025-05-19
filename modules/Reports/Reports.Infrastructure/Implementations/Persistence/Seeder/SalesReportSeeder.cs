using Infrastructure.Implementations.Persistence.Seeders;
using Reports.Domain.Modules.Entities;
using Reports.Infrastructure.Implementations.Persistence.EFCore;

namespace Reports.Infrastructure.Implementations.Persistence.Seeder;

public class SalesReportSeeder : ISeeder<ReportDbContext>
{
    public async Task Seed(ReportDbContext context)
    {
        if (!context.Customers.Any())
        {
            // Crear un nuevo cliente
            var customer = new CustomerInfo
            {
                CustomerName = "Juan Pérez",
                CustomerEmail = "juan.perez@example.com",
                CustomerPhone = "123-456-7890"
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync(); 

            var salesReport = new SalesReport
            {
                ReportTitle = "Reporte de Ventas - Enero 2025",
                GeneratedDate = DateTime.UtcNow,
                ReportPeriod = "Enero 2025",
                CustomerInfoId = customer.Id
            };

            context.SalesReports.Add(salesReport);
            await context.SaveChangesAsync();

            SaleDetail[] saleDetails =
            [
                new() { ProductName = "Producto A", Quantity = 2, UnitPrice = 50.00m, SalesReportId = salesReport.Id },
                new() { ProductName = "Producto B", Quantity = 3, UnitPrice = 75.00m, SalesReportId = salesReport.Id },
                new() { ProductName = "Producto C", Quantity = 1, UnitPrice = 150.00m, SalesReportId = salesReport.Id }
            ];

            context.SaleDetails.AddRange(saleDetails);
            await context.SaveChangesAsync();
        }
    }
}
