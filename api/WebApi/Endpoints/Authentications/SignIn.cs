using Authentications.Application.Authentications.Dtos;
using Authentications.Application.Authentications.SignIn;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Authentications;

internal sealed class SignIn : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Authentications}/{nameof(SignIn)}", async ([FromBody] Request request, [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
        {
            SignInCommand command = request.Adapt<SignInCommand>();

            Result<UserResultDto> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Authentications);
    }
}
