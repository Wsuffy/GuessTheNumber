using GuessTheNumber.Application.Models.DTOs;
using GuessTheNumber.Dal.Abstractions;
using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessTheNumber.Implementation.Commands;

public sealed record RemoveGameConfigurationCommand(Guid SessionId) : IRequest<GameConfigurationDto>;

public class
    RemoveGameConfigurationCommandHandler : IRequestHandler<RemoveGameConfigurationCommand, GameConfigurationDto>
{
    private readonly IGameSessionWriteContext _context;

    public RemoveGameConfigurationCommandHandler(IGameSessionWriteContext context)
    {
        _context = context;
    }

    public async Task<GameConfigurationDto> Handle(RemoveGameConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _context.GameSessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId,
            cancellationToken);

        if (session == null)
            throw new BadRequestException("Данной сессии не существует");

        _context.GameSessions.Remove(session);
        await _context.SaveChangesAsync(cancellationToken);

        return new GameConfigurationDto(session.SessionId, session.TargetValue, session.AttemptCount);
    }
}