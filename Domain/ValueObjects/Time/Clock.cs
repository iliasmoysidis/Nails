using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.ValueObjects;

public sealed class Clock : IClock
{
    public UtcDateTime Now => UtcDateTime.Now();
}