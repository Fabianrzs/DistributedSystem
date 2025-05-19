using Domain.Events;
using MassTransit;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Entities;

namespace Notifications.Application.IntegrationEvents;

public class UserSignUpConsumer(IEmailService emailServices) : IConsumer<UserSignUpEvent>
{
    public async Task Consume(ConsumeContext<UserSignUpEvent> context)
    {
        string emailSubject = "¡Bienvenido a nuestra Aplicación!";
        string emailBody = $@"
        ¡Hola {context.Message.Name}!

        Gracias por registrarte en nuestra aplicación. Nos alegra que hayas decidido unirte a nuestra comunidad. 

        A partir de ahora, podrás disfrutar de todas las funcionalidades que ofrecemos. Si tienes alguna pregunta o necesitas ayuda, no dudes en contactarnos.

        ¡Esperamos que disfrutes tu experiencia con nosotros!

        Atentamente,
        El equipo de soporte de la Aplicación";

        // Enviar el correo con el mensaje mejorado
        await emailServices.SendEmail(new Email
        {
            Subject = emailSubject,
            Body = emailBody,
            To = context.Message.Email
        });
    }
}
