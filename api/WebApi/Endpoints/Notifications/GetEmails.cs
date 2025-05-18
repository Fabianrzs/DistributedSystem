using Notifications.Application.DomainEvents.GetEmails;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Notifications;

public class GetEmails : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Notifications}/{nameof(GetEmails)}", async ([FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            GetEmailsQuery command = new();
            Result<List<EmailDto>> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Notifications)
        .RequireAuthorization();
    }
}
