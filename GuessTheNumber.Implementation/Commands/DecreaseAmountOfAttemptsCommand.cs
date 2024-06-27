using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GuessTheNumber.Implementation.Commands;

public sealed record DecreaseAmountOfAttemptsCommand(Guid SessionId) : IRequest<int>;

public class DecreaseAmountOfAttemptsCommandHandler : IRequestHandler<DecreaseAmountOfAttemptsCommand, int>
{
    private readonly IGameSessionWriteContext _context;
    private readonly ILogger<DecreaseAmountOfAttemptsCommandHandler> _logger;

    public DecreaseAmountOfAttemptsCommandHandler(IGameSessionWriteContext context,
        ILogger<DecreaseAmountOfAttemptsCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<int> Handle(DecreaseAmountOfAttemptsCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);
        try
        {
            var session = await _context.GameSessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId,
                cancellationToken);

            if (session == null)
                throw new BadRequestException("Данной сессии не существует");

            session.AttemptCount -= 1;

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return session.AttemptCount;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogCritical(e.Message);
            throw;
        }
    }
}