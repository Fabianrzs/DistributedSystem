using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstractions.Behaviors;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        logger.LogInformation("Processing request {RequestName}", requestName);

        try
        {
            TResponse result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed request {RequestName} with error", requestName);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            string requestMessage = $"Error processing request {requestName}";
            logger.LogError(ex, "{RequestMessage} with error {Message}", requestMessage, ex.Message);
            throw new Exception(requestMessage, ex); // o una excepción más específica
        }
    }
    
}
