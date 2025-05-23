﻿using Authentications.Application.Authentications.CheckSession;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi.Endpoints.Authentications;
public class CheckSession : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Tags.Authentications}/{nameof(CheckSession)}", async ([FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CheckSessionCommand();
            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Authentications)
        .RequireAuthorization();
    }
}
