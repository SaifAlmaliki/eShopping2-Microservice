using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;

/* The ValidationBehavior class ensures that requests are validated before they are processed by the next handler in the pipeline.
 * It uses the FluentValidation library to perform this validation. 
 * If any validation errors are found, it throws a ValidationException with details of the failures, 
 * thereby preventing the request from being processed further.
 * 
 * */
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

        // If no validation errors are found, the request is passed to the next handler in the pipeline
        return await next();
    }
}
