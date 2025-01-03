namespace Infrastructure.Middlewares;

using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    var response = context.Response;
    response.ContentType = "application/json";

    var errorResponse = new ErrorResponse
    {
      TraceId = context.TraceIdentifier,
      Message = exception.Message
    };

    switch (exception)
    {
      case FluentValidation.ValidationException validationEx:
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        errorResponse.Message = "Validation failed";
        errorResponse.Errors = validationEx.Errors
            .Select(e => new ErrorDetail(e.PropertyName, e.ErrorMessage))
            .ToList();
        break;

      case BusinessRuleException businessEx:
        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
        errorResponse.Details = businessEx.Details;
        break;

      case DomainException:
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        break;

      default:
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        errorResponse.Message = "An unexpected error occurred";
        _logger.LogError(exception, "An unexpected error occurred");
        break;
    }

    var result = JsonSerializer.Serialize(errorResponse);
    await response.WriteAsync(result);
  }
}

public class ErrorResponse
{
  public string TraceId { get; set; }
  public string Message { get; set; }
  public string Details { get; set; }
  public List<ErrorDetail> Errors { get; set; } = new();
}

public record ErrorDetail(string Field, string Message);