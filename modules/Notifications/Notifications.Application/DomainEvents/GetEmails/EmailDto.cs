namespace Notifications.Application.DomainEvents.GetEmails;

public class EmailDto
{
    public string Id { get; set; }

    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> Attachments { get; set; }
}
