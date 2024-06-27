using GuessTheNumber.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace GuessTheNumber.Domain.Extensions;

public static class LoggerExtensions
{
    public static void LogHttpException(this ILogger logger, HttpExceptionWithLog exceptionWithLog) =>
        logger.LogError(exceptionWithLog.LoggingMessage, exceptionWithLog.LoggingParameters!);
}