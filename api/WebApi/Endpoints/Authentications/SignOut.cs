using Authentications.Application.Authentications.SignOut;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Authentications;

internal sealed class SignOut : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Authentications}/{nameof(SignOut)}", async ([FromServices] ISender sender,CancellationToken cancellationToken) =>
        {
            SignOutCommand command = new();

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Authentications)
        .RequireAuthorization();
    }
}
