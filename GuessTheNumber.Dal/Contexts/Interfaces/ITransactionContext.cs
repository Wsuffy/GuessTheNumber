using Microsoft.EntityFrameworkCore.Storage;

namespace GuessTheNumber.Dal.Contexts.Interfaces;

public interface ITransactionContext
{
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}