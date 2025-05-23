﻿using MassTransit;
using Reports.Application.Abstractions.Cache;
using Reports.Application.Abstractions.FileProcessor;
using Reports.Domain.Modules.Entities;
using Reports.Domain.Modules.Repositories;

namespace Reports.Application.Reports.Sales;

internal sealed class SalesCommandHandler(ISalesRepository salesRepository, IFileProcessingService fileProcessing,
    IBus bus,IDistributedCacheService cacheService)
    : ICommandHandler<SalesCommand, MemoryStream>
{
    public async Task<Result<MemoryStream>> Handle(SalesCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = nameof(SalesReport);

        byte[] cachedReport = await cacheService.GetAsync(cacheKey);

        if (cachedReport != null)
        {
            return Result.Success(new MemoryStream(cachedReport));
        }

        var salesReport = (List<SalesReport>)await salesRepository.GetAllAsync();
        List<SalesReportDto> salesFile = MapSalesReportToDto(salesReport);

        MemoryStream reportStream = fileProcessing.GenerateExcelFile(salesFile, cancellationToken);
        byte[] reportBytes = reportStream.ToArray();
        await cacheService.SetAsync(cacheKey, reportBytes);
        await bus.Publish(new SalesReport(), cancellationToken);
        return Result.Success(reportStream);
    }

    private List<SalesReportDto> MapSalesReportToDto(List<SalesReport> salesReport)
    {

        var salesFile = new List<SalesReportDto>();
        foreach (SalesReport report in salesReport)
        {
            foreach (SaleDetail detail in report.SalesDetails)
            {
                var reportDto = new SalesReportDto
                {
                    ReportTitle = report.ReportTitle,
                    GeneratedDate = report.GeneratedDate,
                    ReportPeriod = report.ReportPeriod,
                    CustomerName = report.Customer.CustomerName,
                    CustomerEmail = report.Customer.CustomerEmail,
                    CustomerPhone = report.Customer.CustomerPhone,
                    ProductName = detail.ProductName,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice,
                    TotalPrice = detail.TotalPrice
                };

                salesFile.Add(reportDto);
            }
        }
        return salesFile;
    }
}
