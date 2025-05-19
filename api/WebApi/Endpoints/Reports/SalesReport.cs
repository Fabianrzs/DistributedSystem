using Reports.Application.Reports.Sales;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Reports;
public class SalesReport : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Reports}/{nameof(SalesReport)}", async (
            [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new SalesCommand();
            Result<MemoryStream> result = await sender.Send(command, cancellationToken);

            return result.Match(
                reportStream => Results.File(reportStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"sales_report_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.xlsx"),
                CustomResults.Problem);
        })
            .WithTags(Tags.Reports)
            .DisableAntiforgery();
    }
}
