namespace Notifications.Application.DomainEvents.SendEmail;
public sealed record SendEmailCommand(
    string To,
    string Subject,
    string Body,
    List<string>? Attachments
    ) : ICommand;
