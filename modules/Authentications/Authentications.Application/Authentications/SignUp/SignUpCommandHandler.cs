using Authentications.Application.Authentications.Dtos;

namespace Authentications.Application.Authentications.SignUp;

internal sealed class SignUpCommandHandler(
    IAuthenticationRepository authRepository,
    ITokenProvider tokenProvider)
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

        return new UserResultDto(
            session.Id,
            userResult,
            accessToken,
            refreshToken
        );
    }
}
