using Authentications.Application.Authentications.Dtos;
using Domain.Events;
using MassTransit;

namespace Authentications.Application.Authentications.SignUp;

internal sealed class SignUpCommandHandler(
    IAuthenticationRepository authRepository,
    ITokenProvider tokenProvider, IBus messagePublisher)
    : ICommandHandler<SignUpCommand, UserResultDto>
{
    public async Task<Result<UserResultDto>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {

        User existingUser = await authRepository.FindFirstAsync(x => x.Email == request.Email);

        if (existingUser is not null)
        {
            return Result.Failure<UserResultDto>(AuthenticationErrors.UserAlreadyExists(request.Email));
        }

        User user = request.Adapt<User>();
        await authRepository.SignUpAsync(user);

        Session session = await authRepository.CreateSessionAsync(user.Id);
        UserDto userResult = user.Adapt<UserDto>();

        string accessToken = tokenProvider.GenerateAccessToken(session.Id, userResult);
        string refreshToken = tokenProvider.GenerateRefreshToken(session.Id, userResult.Id);


        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Result.Failure<UserResultDto>(AuthenticationErrors.GenerateTokenInvalid());
        }

        await messagePublisher.Publish(new UserSignUpEvent(user.Id, user.Email, 
            $"{user.FirstName} {user.LastName}"), cancellationToken);

        return new UserResultDto(
            session.Id,
            userResult,
            accessToken,
            refreshToken
        );
    }
}
