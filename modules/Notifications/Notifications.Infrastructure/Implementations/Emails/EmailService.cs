using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Entities;
using Notifications.Domain.Modules.Repositories;
using Notifications.Infrastructure.Settings;

namespace Notifications.Infrastructure.Implementations.Emails;

public class EmailService : IEmailService, IDisposable
{
    private readonly SmtpClient _smtpClient;
    private readonly EmailSettings _emailSettings;
    private readonly IEmailRepository _emailRepository;

    public EmailService(IOptions<EmailSettings> options, IEmailRepository emailRepository)
    {
        _emailSettings = options.Value;
        _emailRepository = emailRepository;

        _smtpClient = new SmtpClient();
        _smtpClient.Connect(_emailSettings.Host, _emailSettings.PortTLS, SecureSocketOptions.StartTls);
        _smtpClient.Authenticate(_emailSettings.Username, _emailSettings.Password);
    }

    public async Task SendEmail(Email request)
    {
        using MimeMessage email = CreateEmailMessage(request.To, request.Subject, request.Body);
        await SendAsync(email);
        await _emailRepository.SaveAsync(request);
    }

    public async Task SendCopyEmail(EmailCopy request)
    {
        using MimeMessage email = CreateEmailMessage(request.To, request.Subject, request.Body);
        email.Cc.Add(MailboxAddress.Parse(request.Cc));
        email.Bcc.Add(MailboxAddress.Parse(request.Bcc));
        await SendAsync(email);
        await _emailRepository.SaveAsync(request);
    }

    public async Task SendPriorityEmail(EmailPriority request)
    {
        using MimeMessage email = CreateEmailMessage(request.To, request.Subject, request.Body);
        email.Priority = (MessagePriority)request.PriorityLevel;
        await SendAsync(email);
        await _emailRepository.SaveAsync(request);
    }

    private MimeMessage CreateEmailMessage(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_emailSettings.Username));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body };
        return message;
    }

    private async Task SendAsync(MimeMessage message)
    {
        await _smtpClient.SendAsync(message);
        await _smtpClient.DisconnectAsync(true);
    }

    public void Dispose()
    {
        _smtpClient?.Dispose();
    }
}
