using System.Net;

namespace speedupApi.Exceptions
{
  public class ExceptionsHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionsHandlingMiddleware> _logger;

    public ExceptionsHandlingMiddleware(RequestDelegate next, ILogger<ExceptionsHandlingMiddleware> logger)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleUnhandledExceptionAsync(context, ex);
      }
    }

    private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex)
    {
      _logger.LogError(ex, ex.Message);
      if (!context.Response.HasStarted)
      {
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string message = string.Empty;
#if DEBUG
        message = ex.Message;
#else
        message = "An unhandled exception has occurred";
#endif
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var result = new ExceptionMessage(message).ToString();
        await context.Response.WriteAsync(result);
      }
    }
  }
}
