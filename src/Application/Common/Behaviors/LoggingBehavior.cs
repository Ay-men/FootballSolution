namespace Application.Common.Behaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _timer;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestGuid = Guid.NewGuid().ToString();

        try
        {
            _logger.LogInformation(
                "Beginning request {@RequestName} {@RequestGuid}",
                requestName,
                requestGuid);

            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds <= 500)
            {
                _logger.LogInformation(
                    "Completed request {@RequestName} {@RequestGuid} in {@ElapsedMilliseconds}ms",
                    requestName,
                    requestGuid,
                    _timer.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning(
                    "Long running request {@RequestName} {@RequestGuid} in {@ElapsedMilliseconds}ms",
                    requestName,
                    requestGuid,
                    _timer.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            _timer.Stop();

            _logger.LogError(
                ex,
                "Request failure {@RequestName} {@RequestGuid} in {@ElapsedMilliseconds}ms {@Error}",
                requestName,
                requestGuid,
                _timer.ElapsedMilliseconds,
                ex.Message);

            throw;
        }
    }
}