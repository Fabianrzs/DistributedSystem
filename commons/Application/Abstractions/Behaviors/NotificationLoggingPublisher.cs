using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstractions.Behaviors;

public sealed class DomainNotificationLoggingPublisher(ILogger<DomainNotificationLoggingPublisher> logger)
    : INotificationPublisher
{
    public async Task Publish(
        IEnumerable<NotificationHandlerExecutor> handlerExecutors,
        INotification notification,
        CancellationToken cancellationToken)
    {

        string notificationName = notification.GetType().Name;

        foreach (NotificationHandlerExecutor handlerExecutor in handlerExecutors)
        {
            string handlerTypeName = handlerExecutor.HandlerInstance.GetType().FullName ?? "UnknownHandler";

            try
            {
                logger.LogInformation("Executing handler: {HandlerType}", handlerTypeName);

                await handlerExecutor.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);

                logger.LogInformation("Handler completed: {HandlerType}", handlerTypeName);
            }
            catch (Exception exception)
            {
                using (LogContext.PushProperty("Notification", notificationName))
                using (LogContext.PushProperty("Handler", handlerTypeName))
                {
                    logger.LogError(
                        exception,
                        "Error processing notification {NotificationName} in handler {HandlerType}: {Message}",
                        notificationName,
                        handlerTypeName,
                        exception.Message);
                }
            }
        }

        logger.LogInformation("Finished handling notification: {NotificationName}", notificationName);
    }
}
