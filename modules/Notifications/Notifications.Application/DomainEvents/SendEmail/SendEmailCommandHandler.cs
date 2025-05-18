using Application.Abstractions.Security;
using Domain.Abstractions.Errors;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Entities;

namespace Notifications.Application.DomainEvents.SendEmail;

internal sealed class SendEmailCommandHandler(IEmailService emailService, IUserContext userContext)
    : ICommandHandler<SendEmailCommand>
{
    public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        Email email = request.Adapt<Email>();
        email.To = userContext.Email;
        await emailService.SendEmail(email);
        return Result.Success();
    }
}
