using GuessTheNumber.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessTheNumber.Dal.Implementation.Configurations;

public class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable("GameSessions");
        builder.HasKey(x => x.SessionId);

        builder.Property(x => x.TargetValue).IsRequired();
        builder.Property(x => x.AttemptCount).IsRequired();
    }
}