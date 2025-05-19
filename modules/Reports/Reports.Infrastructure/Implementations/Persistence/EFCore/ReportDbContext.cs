using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.Modules.Entities;

namespace Reports.Infrastructure.Implementations.Persistence.EFCore;

public class ReportDbContext(DbContextOptions<ReportDbContext> options) : ApplicationsDbContext(options)
{
    public DbSet<SalesReport> SalesReports { get; set; }
    public DbSet<CustomerInfo> Customers { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SalesReport>()
            .HasMany(r => r.SalesDetails)
            .WithOne(d => d.SalesReport)
            .HasForeignKey(d => d.SalesReportId);

        modelBuilder.Entity<SalesReport>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerInfoId);
    }
}
