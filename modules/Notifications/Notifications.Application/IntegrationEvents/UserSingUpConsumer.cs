using Domain.Events;
using MassTransit;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Entities;

namespace Notifications.Application.IntegrationEvents;

public class UserSingUpConsumer(IEmailService emailServices) : IConsumer<UserSignUpEvent>
{
    public async Task Consume(ConsumeContext<UserSignUpEvent> context)
    {
        await emailServices.SendEmail(new Email
        {
            Subject = "Nuevo Registro Realizado",
            Body = "Bienvenido A la Aplicacion, Esperamos que sea de mucho entuciasmo para ti.",
            To = context.Message.Email            
        });

    }
}
