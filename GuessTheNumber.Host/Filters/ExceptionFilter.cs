using GuessTheNumber.Domain.Exceptions;
using GuessTheNumber.Domain.Extensions;
using GuessTheNumber.Domain.Http;
using GuessTheNumber.Domain.Http.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuessTheNumber.Host.Filters;

/// <inheritdoc />
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task OnExceptionAsync(ExceptionContext exceptionContext)
    {
        var context = exceptionContext.HttpContext;
        var ex = exceptionContext.Exception;

        if (await ProcessException(ex, context))
            exceptionContext.ExceptionHandled = true;
    }

    private async Task<bool> ProcessException(Exception exception, HttpContext context)
    {
        switch (exception)
        {
            case HttpExceptionWithLog exceptionWithLog:
            {
                if (!string.IsNullOrEmpty(exceptionWithLog.LoggingMessage))
                    _logger.LogHttpException(exceptionWithLog);

                await context.WriteResponseToContextAsync(new ErrorResponse(exceptionWithLog),
                    exceptionWithLog.StatusCode!.Value);

                return true;
            }
            case HttpRequestException badRequestException:
                await context.WriteResponseToContextAsync(new ErrorResponse(badRequestException),
                    badRequestException.StatusCode);

                return true;

            default:
                return false;
        }
    }
}