using Domain.Abstractions.Errors;
using Notifications.Domain.Modules.Entities;
using Notifications.Domain.Modules.Repositories;

namespace Notifications.Application.DomainEvents.GetEmails;

internal sealed class GetEmailsQueryHandler(IEmailRepository repository)
    : IQueryHandler<GetEmailsQuery, List<EmailDto>>
{
    public async Task<Result<List<EmailDto>>> Handle(GetEmailsQuery request, CancellationToken cancellationToken)
    {
        List<Email> emails = await repository.GetAllAsync();
        return emails.Adapt<List<EmailDto>>();
    }
}
