using Domain.Abstractions.Entities;

namespace Reports.Domain.Modules.Entities;
public class SaleDetail : Entity
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    // Clave foránea a SalesReport
    public Guid SalesReportId { get; set; }
    public SalesReport SalesReport { get; set; }  // Relación de muchos a uno
}
