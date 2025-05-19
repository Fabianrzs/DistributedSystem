namespace Reports.Application.Reports.Sales;

public class SalesReportDto
{
    public string ReportTitle { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string ReportPeriod { get; set; }

    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }

    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
