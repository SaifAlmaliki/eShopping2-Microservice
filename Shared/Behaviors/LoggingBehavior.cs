using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

// LoggingBehavior is a MediatR pipeline behavior that logs detailed information about the request handling process.
// It logs the start and end of a request, the request data, the response, and the time taken to handle the request.
// It also logs a warning if the request handling takes more than a specified threshold (3 seconds in this case).

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>  // Constraint to ensure TRequest is not nullable and implements IRequest<TResponse>
    where TResponse : notnull                      // Constraint to ensure TResponse is not nullable
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private const int WarningThresholdSeconds = 3; // Threshold for logging performance warnings

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Log the start of the request handling with detailed information
        _logger.LogInformation("[START] Handling request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        // Start a timer to measure the execution time of the request handling
        var timer = Stopwatch.StartNew();

        // Invoke the next delegate/middleware in the pipeline
        TResponse response;
        try
        {
            response = await next();
        }
        catch (Exception ex)
        {
            // Log exception details if an error occurs during request handling
            timer.Stop();
            _logger.LogError(ex, "[ERROR] Exception occurred while handling request={Request} - RequestData={RequestData}",
                typeof(TRequest).Name, request);
            throw; // Re-throw the exception to ensure the error is propagated
        }

        // Stop the timer after the request handling is complete
        timer.Stop();
        var timeTaken = timer.Elapsed;

        // Log the time taken to handle the request
        _logger.LogInformation("[TIMING] The request {Request} took {TimeTakenMilliseconds} ms to process.",
            typeof(TRequest).Name, timeTaken.TotalMilliseconds);

        // If the request handling took more than the warning threshold, log a performance warning
        if (timeTaken.TotalSeconds > WarningThresholdSeconds)
        {
            _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTakenSeconds} seconds, which exceeds the threshold of {WarningThresholdSeconds} seconds.",
                typeof(TRequest).Name, timeTaken.TotalSeconds, WarningThresholdSeconds);
        }

        // Log the end of the request handling with response details
        _logger.LogInformation("[END] Handled {Request} with {Response} - ResponseData={ResponseData}",
            typeof(TRequest).Name, typeof(TResponse).Name, response);

        return response;
    }
}
