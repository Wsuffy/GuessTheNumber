namespace GuessTheNumber.Dal.Contexts.Interfaces;

public interface ISavableContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}