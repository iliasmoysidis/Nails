namespace Application.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
}