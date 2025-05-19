using Authentications.Application.Authentications.Dtos;
using Domain.Events;
using MassTransit;

namespace Authentications.Application.Authentications.SignIn;

internal sealed class SignInCommandHandler(IAuthenticationRepository authRepository, 
    ITokenProvider tokenProvider, IBus messagePublisher)
    : ICommandHandler<SignInCommand, UserResultDto>
{
    public async Task<Result<UserResultDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        User? user = await authRepository.SignInAsync(request.Email, request.Password);
        if (user is null)
        {
            return Result.Failure<UserResultDto>(AuthenticationErrors.InvalidCredentials());
        }

        UserDto userResult = user.Adapt<UserDto>();

        Session session = await authRepository.CreateSessionAsync(user.Id);

        string accessToken = tokenProvider.GenerateAccessToken(session.Id, userResult);
        string refreshToken = tokenProvider.GenerateRefreshToken(session.Id, userResult.Id);


        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Result.Failure<UserResultDto>(AuthenticationErrors.GenerateTokenInvalid());
        }

        await messagePublisher.Publish(new UserSignInEvent(user.Id, user.Email,
            $"{user.FirstName} {user.LastName}"), cancellationToken: cancellationToken);

        return new UserResultDto(
            session.Id,
            userResult,
            accessToken,
            refreshToken
        );
    }
}
