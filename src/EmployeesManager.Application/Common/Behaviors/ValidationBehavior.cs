using EmployeesManager.Domain.Common.Results;
using FluentValidation;
using MediatR;

namespace EmployeesManager.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = (
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)))
        )
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var errors = failures.ConvertAll(f =>
        {
            var propertyName = string.IsNullOrWhiteSpace(f.PropertyName) ? null : f.PropertyName;

            return Error.Validation(
                code: $"Validation.{propertyName ?? "Unknown"}",
                description: f.ErrorMessage,
                propertyName: propertyName
            );
        });

        return (dynamic)errors;
    }
}
