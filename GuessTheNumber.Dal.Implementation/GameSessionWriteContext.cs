using GuessTheNumber.Dal.Abstractions;
using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Dal.Implementation.Configurations;
using GuessTheNumber.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GuessTheNumber.Dal.Implementation;

public class GameSessionWriteContext : DbContext, IGameSessionWriteContext
{
    public DbSet<GameSession> GameSessions => Set<GameSession>();

    public GameSessionWriteContext(DbContextOptions<GameSessionWriteContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameSessionConfiguration());
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await Database.BeginTransactionAsync(cancellationToken);
}