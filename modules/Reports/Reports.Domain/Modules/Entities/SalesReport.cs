using Domain.Abstractions.Entities;

namespace Reports.Domain.Modules.Entities;
public class SalesReport: Entity
{
    public string ReportTitle { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string ReportPeriod { get; set; }
    public CustomerInfo Customer { get; set; }
    public List<SaleDetail> SalesDetails { get; set; }
}
