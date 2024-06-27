using System.Net;

namespace GuessTheNumber.Domain.Exceptions;

public class BadRequestExceptionWithLog : HttpExceptionWithLog
{
    public BadRequestExceptionWithLog(string reasonPhrase, string loggingMessage,
        params object[]? loggingParameters) : base(reasonPhrase, reasonPhrase, loggingMessage, HttpStatusCode.BadRequest,
        loggingParameters)
    {
    }
}

public class BadRequestException : HttpRequestException
{
    public BadRequestException(string reasonPhrase, Exception? inner = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(reasonPhrase, inner, statusCode)
    {
    }
}