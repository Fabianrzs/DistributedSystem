using Notifications.Domain.Modules.Entities;

namespace Notifications.Domain.Modules.Repositories;

public interface IEmailRepository
{
    Task SaveAsync(Email email);
    Task<List<Email>> GetAllAsync();
}
