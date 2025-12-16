using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Common;

public abstract class BaseEntity
{
    public UtcDateTime CreatedAt { get; private set; }
    public UtcDateTime? UpdatedAt { get; private set; }

    protected void MarkAsCreated(IClock clock)
    {
        CreatedAt = clock.Now;
    }

    public void MarkAsUpdated(IClock clock)
    {
        UpdatedAt = clock.Now;
    }
}