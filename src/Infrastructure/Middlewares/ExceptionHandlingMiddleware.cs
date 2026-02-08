using Finances.Domain.Result;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using ILogger = Serilog.ILogger;

namespace Infrastructure.Middlewares;
public class ExceptionHandlingMiddleware(ILogger logger, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
                await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        logger.Error(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException => BaseResult.Failure((int)HttpStatusCode.Unauthorized, errorMessage),
            _ => BaseResult.Failure((int)HttpStatusCode.InternalServerError, errorMessage),
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = (int)response.Error.Code;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
