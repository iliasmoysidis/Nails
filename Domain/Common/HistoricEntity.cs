using Domain.Exceptions;
using Domain.ValueObjects.Time;

namespace Domain.Common;

public abstract class HistoricEntity : BaseEntity
{
    public UtcDateTime? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    protected HistoricEntity() { }

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new DomainException($"{GetType().Name} is already deleted.");
        }

        DeletedAt = UtcDateTime.Now();
        MarkAsUpdated();
    }

    public void Restore()
    {
        if (!IsDeleted)
        {
            throw new DomainException($"{GetType().Name} is not deleted.");
        }

        DeletedAt = null;
        MarkAsUpdated();
    }
}