using Reports.Domain.Modules.Entities;

namespace Reports.Application.Reports.Sales;

public class SaleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SalesReport, SalesReportDto>()
            .Map(dest => dest.ReportTitle, src => src.ReportTitle)
            .Map(dest => dest.GeneratedDate, src => src.GeneratedDate)
            .Map(dest => dest.ReportPeriod, src => src.ReportPeriod)
            .Map(dest => dest.CustomerName, src => src.Customer.CustomerName)
            .Map(dest => dest.CustomerEmail, src => src.Customer.CustomerEmail)
            .Map(dest => dest.CustomerPhone, src => src.Customer.CustomerPhone)
            .Map(dest => dest.ProductName, src => src.SalesDetails.Select(d => d.ProductName).ToList())
            .Map(dest => dest.Quantity, src => src.SalesDetails.Select(d => d.Quantity).ToList())
            .Map(dest => dest.UnitPrice, src => src.SalesDetails.Select(d => d.UnitPrice).ToList())
            .Map(dest => dest.TotalPrice, src => src.SalesDetails.Select(d => d.TotalPrice).ToList());
    }
}
