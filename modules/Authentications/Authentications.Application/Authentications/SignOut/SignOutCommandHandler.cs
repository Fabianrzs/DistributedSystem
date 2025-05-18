namespace Authentications.Application.Authentications.SignOut;

internal sealed class SignOutCommandHandler(
    IAuthenticationRepository authRepository, IUserContext userContext)
    : ICommandHandler<SignOutCommand>
{
    public async Task<Result> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await authRepository.SignOutAsync(userContext.SessionId);
        return Result.Success();
    }
}
