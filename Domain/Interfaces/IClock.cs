using Domain.ValueObjects.Time;

namespace Domain.Interfaces;

public interface IClock
{
    UtcDateTime Now { get; }
}