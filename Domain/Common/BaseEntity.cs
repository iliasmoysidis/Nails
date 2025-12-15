using Domain.ValueObjects.Time;

namespace Domain.Common;

public abstract class BaseEntity
{
    public UtcDateTime CreatedAt { get; private set; }
    public UtcDateTime? UpdatedAt { get; private set; }

    protected BaseEntity()
    {
        CreatedAt = UtcDateTime.Now();
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = UtcDateTime.Now();
    }
}