using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifications.Domain.Modules.Entities;
using Notifications.Domain.Modules.Repositories;
using Notifications.Infrastructure.Settings;

namespace Notifications.Infrastructure.Implementations.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly IMongoCollection<Email> emails;

    public EmailRepository(IOptions<MongoDbSettings> options)
    {
        #pragma warning disable CA2000 // Dispose objects before losing scope
        var client = new MongoClient(options.Value.ConnectionString);
        #pragma warning restore CA2000
        IMongoDatabase database = client.GetDatabase(options.Value.DatabaseName);
        emails = database.GetCollection<Email>("Emails");
    }

    public async Task SaveAsync(Email email)
    {
        await emails.InsertOneAsync(email);
    }

    public async Task<List<Email>> GetAllAsync()
    {
        return await emails.Find(_ => true).ToListAsync();
    }
}
