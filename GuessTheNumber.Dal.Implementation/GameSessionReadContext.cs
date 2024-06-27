using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Dal.Implementation.Configurations;
using GuessTheNumber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GuessTheNumber.Dal.Implementation;

public class GameSessionReadContext : DbContext, IGameSessionReadContext
{
    public IQueryable<GameSession> GameSessions => Set<GameSession>();

    public GameSessionReadContext(DbContextOptions<GameSessionReadContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameSessionConfiguration());
    }
}