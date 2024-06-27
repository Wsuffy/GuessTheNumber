using GuessTheNumber.Dal.Contexts.Interfaces;
using GuessTheNumber.Domain.Entities;

namespace GuessTheNumber.Dal.Abstractions.Interfaces;

public interface IGameSessionReadContext : ISavableContext
{
    public IQueryable<GameSession> GameSessions { get; }
}