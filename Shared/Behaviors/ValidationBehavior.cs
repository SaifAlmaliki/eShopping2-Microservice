using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;

// Define a validation behavior for the pipeline
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    // Collection of validators for the request
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // Constructor to inject validators
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    // Handle method to process the request
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Create a validation context for the request
        var context = new ValidationContext<TRequest>(request);

        // Execute all validators and collect validation results
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Gather all validation failures
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        // If there are any validation failures, throw a validation exception
        if (failures.Any())
            throw new ValidationException(failures);

        // Proceed to the next handler in the pipeline
        return await next();
    }
}
