using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Request was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred while processing request.");
            throw;
        }
    }
}
