namespace Notifications.Application.DomainEvents.GetEmails;

public sealed record GetEmailsQuery() : IQuery<List<EmailDto>>;
