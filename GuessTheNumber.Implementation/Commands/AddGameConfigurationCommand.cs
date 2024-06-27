using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.DTOs;
using GuessTheNumber.Dal.Abstractions;
using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Domain.Entities;
using MediatR;

namespace GuessTheNumber.Implementation.Commands;

public sealed record AddGameConfigurationCommand(int LeftBorder, int RightBorder, int Attempts)
    : IRequest<GameConfigurationDto>;

public class AddGameConfigurationCommandHandler : IRequestHandler<AddGameConfigurationCommand, GameConfigurationDto>
{
    private readonly IGuessRandomNumberGenerator _randomNumberGenerator;
    private readonly IGameSessionWriteContext _context;

    public AddGameConfigurationCommandHandler(IGuessRandomNumberGenerator randomNumberGenerator,
        IGameSessionWriteContext context)
    {
        _randomNumberGenerator = randomNumberGenerator;
        _context = context;
    }

    public async Task<GameConfigurationDto> Handle(AddGameConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        var targetValue = _randomNumberGenerator.Generate(request.LeftBorder, request.RightBorder);

        var entity = new GameSession
        {
            TargetValue = targetValue,
            AttemptCount = request.Attempts
        };

        await _context.GameSessions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new GameConfigurationDto(entity.SessionId, entity.TargetValue, entity.AttemptCount);
    }
}