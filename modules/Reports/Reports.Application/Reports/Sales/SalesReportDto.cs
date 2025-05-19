using Reports.Domain.Attributes.Excel;

namespace Reports.Application.Reports.Sales;

public class SalesReportDto
{
    [ExcelColumnNumber(1), ExcelColumnName("Titulo")]
    public string ReportTitle { get; set; }
    [ExcelColumnNumber(2), ExcelColumnName("Fecha")]
    public DateTime GeneratedDate { get; set; }
    [ExcelColumnNumber(3), ExcelColumnName("Periodo")]
    public string ReportPeriod { get; set; }
    [ExcelColumnNumber(4), ExcelColumnName("Nombre del cliente")]
    public string CustomerName { get; set; }
    [ExcelColumnNumber(5), ExcelColumnName("Email del cliente")]
    public string CustomerEmail { get; set; }
    [ExcelColumnNumber(6), ExcelColumnName("Telefono del cliente")]
    public string CustomerPhone { get; set; }
    [ExcelColumnNumber(7), ExcelColumnName("Nombre del Producto")]
    public string ProductName { get; set; }
    [ExcelColumnNumber(8), ExcelColumnName("Cantidad")]
    public int Quantity { get; set; }
    [ExcelColumnNumber(9), ExcelColumnName("Precio")]
    public decimal UnitPrice { get; set; }
    [ExcelColumnNumber(10), ExcelColumnName("Precio Total")]
    public decimal TotalPrice { get; set; }
}
