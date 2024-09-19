using Serilog;

namespace NTier.Presentation.Middlewares;

public class RequestTimeout
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timeout;

    public RequestTimeout(RequestDelegate next, TimeSpan timeout)
    {
        _next = next;
        _timeout = timeout;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var cts = new CancellationTokenSource())
        {
            cts.CancelAfter(_timeout);
            var token = cts.Token;

            try
            {
                Log.Information("Starting request with timeout set to {Timeout} seconds.", _timeout.TotalSeconds);
                context.RequestAborted = token;
                await _next(context);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                Log.Error("Request timed out after {Timeout} seconds. OperationCanceledException occurred.", _timeout.TotalSeconds);
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                await context.Response.WriteAsync("Request timed out.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while processing the request.");
                throw;
            }
        }
    }
}

// Extension method to add the middleware
public static class RequestTimeoutMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder builder, TimeSpan timeout)
    {
        return builder.UseMiddleware<RequestTimeout>(timeout);
    }
}