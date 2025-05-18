namespace Notifications.Infrastructure.Settings;

public class EmailSettings
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int PortTLS { get; set; }
    public int PortSSL { get; set; }
}
