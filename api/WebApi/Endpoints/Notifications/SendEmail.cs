using Notifications.Application.DomainEvents.SendEmail;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Notifications;
public class SendEmail : IEndpoint
{
    public class Request
    {
        public string To { get; set; }
        public string Subject{ get; set; }
        public string Body { get; set; }
        public List<string>? Attachments { get; set; }
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Notifications}/{nameof(SendEmail)}", async ([FromBody] Request request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            SendEmailCommand command = request.Adapt<SendEmailCommand>();
            Result result = await sender.Send(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Notifications)
        .RequireAuthorization();
    }
}
