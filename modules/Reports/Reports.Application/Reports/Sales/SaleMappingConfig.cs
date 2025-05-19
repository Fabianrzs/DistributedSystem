using Reports.Domain.Modules.Entities;

namespace Reports.Application.Reports.Sales;

public static class SaleMappingConfig
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<SalesReport, SalesReportDto>.NewConfig()
            .Map(dest => dest.ReportTitle, src => src.ReportTitle)
            .Map(dest => dest.GeneratedDate, src => src.GeneratedDate)
            .Map(dest => dest.ReportPeriod, src => src.ReportPeriod)
            .Map(dest => dest.CustomerName, src => src.Customer.CustomerName)
            .Map(dest => dest.CustomerEmail, src => src.Customer.CustomerEmail)
            .Map(dest => dest.CustomerPhone, src => src.Customer.CustomerPhone)
            .Map(dest => dest.ProductName, src => "") 
            .Map(dest => dest.Quantity, src => 0)
            .Map(dest => dest.UnitPrice, src => 0m) 
            .Map(dest => dest.TotalPrice, src => 0m);

        TypeAdapterConfig<SaleDetail, SalesReportDto>.NewConfig()
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice)
            .Map(dest => dest.ReportTitle, src => "")
            .Map(dest => dest.GeneratedDate, src => DateTime.MinValue) 
            .Map(dest => dest.ReportPeriod, src => "")
            .Map(dest => dest.CustomerName, src => "")
            .Map(dest => dest.CustomerEmail, src => "") 
            .Map(dest => dest.CustomerPhone, src => "");
    }
}
