using GuessTheNumber.Domain.Exceptions;

namespace GuessTheNumber.Domain.Http.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(Exception exception)
        {
            Error = ErrorMessages.UnknownServerErrorMessage;
            Details = exception.Message;
        }

        public ErrorResponse(BadRequestException exception)
        {
            Error = exception.Message;
            Details = exception.InnerException?.Message;
        }

        public ErrorResponse(HttpExceptionWithLog exception)
        {
            Error = exception.ReasonPhrase;
            Details = exception.Message;
        }


        public string Error { get; }
        public string? Details { get; }
    }
}