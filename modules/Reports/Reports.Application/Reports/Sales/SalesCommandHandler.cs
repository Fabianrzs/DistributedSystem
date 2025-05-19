using MassTransit;
using Microsoft.AspNetCore.Http;
using Reports.Application.Abstractions.FileProcessor;
using Reports.Domain.Modules.Entities;
using Reports.Domain.Modules.Repositories;

namespace Reports.Application.Reports.Sales;

internal sealed class SalesCommandHandler(ISalesRepository salesRepository, IFileProcessingService fileProcessing,IBus bus)
    : ICommandHandler<SalesCommand, IFormFile>
{
    public async Task<Result<IFormFile>> Handle(SalesCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<SalesReport> salesReport = await salesRepository.GetAllAsync();
        IFormFile report = fileProcessing.GenerateExcelFile(salesReport.Adapt<List<SalesReportDto>>(),cancellationToken);
        await bus.Publish(new SalesReport(), cancellationToken);
        return Result.Success(report);
    }
}
