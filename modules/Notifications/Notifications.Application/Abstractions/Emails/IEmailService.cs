using Notifications.Domain.Modules.Entities;

namespace Notifications.Application.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmail(Email request);
    Task SendCopyEmail(EmailCopy request);
    Task SendPriorityEmail(EmailPriority request);
}
