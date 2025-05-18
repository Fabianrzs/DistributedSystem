using Authentications.Application.Authentications.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using TryAdminBack.Application.UseCase.Auth.RefreshToken;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Authentications;

public class RefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Authentications}/{nameof(RefreshToken)}", async ([FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommand();
            Result<RefreshTokenDto> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Authentications)
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = "Refresh" });
    }
}
