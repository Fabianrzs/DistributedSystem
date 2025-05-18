using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notifications.Domain.Modules.Entities;

public class Email
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> Attachments { get; set; }
}
