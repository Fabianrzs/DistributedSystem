using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.Modules.Entities;

namespace Reports.Infrastructure.Implementations.Persistence.EFCore;

public class ReportDbContext(DbContextOptions<ReportDbContext> options) : ApplicationsDbContext(options)
{
    public DbSet<SalesReport> SalesReports { get; set; }
    public DbSet<CustomerInfo> Customers { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
}
