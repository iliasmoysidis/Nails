using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Common;

public abstract class HistoricEntity : BaseEntity
{
    public UtcDateTime? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    public void SoftDelete(IClock clock)
    {
        if (IsDeleted)
        {
            throw new DomainException($"{GetType().Name} is already deleted.");
        }

        DeletedAt = clock.Now;
        MarkAsUpdated(clock);
    }

    public void Restore(IClock clock)
    {
        if (!IsDeleted)
        {
            throw new DomainException($"{GetType().Name} is not deleted.");
        }

        DeletedAt = null;
        MarkAsUpdated(clock);
    }
}