using GuessTheNumber.Dal.Contexts.Interfaces;
using GuessTheNumber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GuessTheNumber.Dal.Abstractions.Interfaces;

public interface IGameSessionWriteContext : ISavableContext, ITransactionContext
{
    public DbSet<GameSession> GameSessions { get; }
}