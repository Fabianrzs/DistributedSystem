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
            Result<IFormFile> result = await sender.Send(command, cancellationToken);

            return result.Match(
                file => Results.File(file.OpenReadStream(), file.ContentType, file.FileName),
                CustomResults.Problem);
        })
        .WithTags(Tags.Reports)
        .DisableAntiforgery()
        .RequireAuthorization();
    }
}
