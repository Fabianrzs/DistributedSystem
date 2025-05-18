using Authentications.Application.Authentications.Dtos;
using TryAdminBack.Application.UseCase.Auth.RefreshToken;

namespace Authentications.Application.Authentications.RefreshToken;

internal sealed class RefreshTokenCommandHandler(IAuthenticationRepository authRepository, 
        ITokenProvider tokenProvider, IUserContext userContext)
            : ICommandHandler<RefreshTokenCommand, RefreshTokenDto>
{
    public async Task<Result<RefreshTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        bool isValidSession = await authRepository.CheckSessionAsync(userContext.UserId, userContext.SessionId);
        if (!isValidSession)
        {
            return Result.Failure<RefreshTokenDto>(AuthenticationErrors.SessionNotFound(userContext.SessionId));
        }

        User? user = await authRepository.FindFirstAsync(x => x.Id == userContext.UserId);

        if (user is null)
        {
            return Result.Failure<RefreshTokenDto>(AuthenticationErrors.UserNotFound(userContext.UserId));
        }

        Session session = await authRepository.CreateSessionAsync(user.Id);

        UserDto userResult = user.Adapt<UserDto>();

        string accessToken = tokenProvider.GenerateAccessToken(session.Id, userResult);
        string refreshToken = tokenProvider.GenerateRefreshToken(session.Id, userResult.Id);

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Result.Failure<RefreshTokenDto>(AuthenticationErrors.GenerateTokenInvalid());
        }

        return new RefreshTokenDto(
            accessToken,
            refreshToken
        );
    }
}
