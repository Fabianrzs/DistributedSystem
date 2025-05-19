using Domain.Events;
using MassTransit;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Entities;

namespace Notifications.Application.IntegrationEvents;

public class UserSignInConsumer(IEmailService emailServices) : IConsumer<UserSignInEvent>
{
    public async Task Consume(ConsumeContext<UserSignInEvent> context)
    {
        string emailSubject = "Bienvenido a nuestra Aplicación";
        string emailBody = $@"
        ¡Hola {context.Message.Name}!

        Nos complace informarte que tu inicio de sesión ha sido exitoso. Ahora puedes acceder a todas las funcionalidades de nuestra aplicación.

        Si no reconoces este inicio de sesión o tienes alguna pregunta, no dudes en contactarnos. ¡Estamos aquí para ayudarte!

        Atentamente,
        El equipo de soporte de la Aplicación";

        await emailServices.SendEmail(new Email
        {
            Subject = emailSubject,
            Body = emailBody,
            To = context.Message.Email
        });
    }
}
