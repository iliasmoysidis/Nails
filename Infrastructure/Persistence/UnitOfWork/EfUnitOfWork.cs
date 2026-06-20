using Domain.Common;
using Application.Abstractions.Events;
using Application.Abstractions.UnitOfWork;

namespace Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly IDomainEventDispatcher _dispatcher;

    public EfUnitOfWork(AppDbContext db, IDomainEventDispatcher dispatcher)
    {
        _db = db;
        _dispatcher = dispatcher;
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        await _db.Database.BeginTransactionAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _db.SaveChangesAsync(ct);

        await DispatchDomainEvents(ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        await _db.Database.CommitTransactionAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        await _db.Database.RollbackTransactionAsync(ct);
    }

    private async Task DispatchDomainEvents(CancellationToken ct)
    {
        var entities = _db.ChangeTracker.Entries<Entity>().Where(e => e.Entity.DomainEvents.Any()).ToList();

        var events = entities.SelectMany(e => e.Entity.DomainEvents).ToList();

        foreach (var entity in entities)
        {
            entity.Entity.ClearDomainEvents();
        }

        await _dispatcher.DispatchAsync(events, ct);
    }
}