namespace API.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
{
  private const string ApiKeyHeaderName = "X-API-Key";

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
    {
      context.Result = new UnauthorizedObjectResult("API Key missing");
      return;
    }

    var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
    var apiKey = configuration.GetValue<string>("ApiKey");

    if (apiKey != null && !apiKey.Equals(extractedApiKey))
    {
      context.Result = new UnauthorizedObjectResult("Invalid API Key");
      return;
    }

    await next();
  }
}