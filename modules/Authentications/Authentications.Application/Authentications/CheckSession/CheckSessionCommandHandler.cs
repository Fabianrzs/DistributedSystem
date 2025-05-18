
namespace Authentications.Application.Authentications.CheckSession;

internal sealed class CheckSessionCommandHandler(IAuthenticationRepository authRepository, IUserContext userContext)
    : ICommandHandler<CheckSessionCommand>
{
    public async Task<Result> Handle(CheckSessionCommand request, CancellationToken cancellationToken)
    {
        bool isValid = await authRepository.CheckSessionAsync(userContext.UserId, userContext.SessionId);
        if (!isValid)
        {
            return Result.Failure(AuthenticationErrors.SessionExpired(userContext.SessionId));
        }
        return Result.Success();
    }
}
