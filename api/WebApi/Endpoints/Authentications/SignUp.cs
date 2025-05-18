using Authentications.Application.Authentications.Dtos;
using Authentications.Application.Authentications.SignUp;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Authentications;

internal sealed class SignUp : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Authentications}/{nameof(SignUp)}", async ([FromBody] Request request, 
            [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            SignUpCommand command = request.Adapt<SignUpCommand>();

            Result<UserResultDto> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Authentications);
    }
}
