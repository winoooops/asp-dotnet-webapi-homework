namespace MyWebAPI.Middleware;

public class RequestLoggingMiddle: IMiddleware
{
  public Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    Console.WriteLine(context.Request.Method);
    Console.WriteLine(context.Request.Path);
    Console.WriteLine(context.Request.QueryString);
    return next(context);
  }
}

public static class RequestLoggingMiddlewareExtensions
{
  public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<RequestLoggingMiddle>();
  }
}