namespace Domain.Common.ValueObjects;

public sealed class Clock : IClock
{
    public UtcDateTime Now => UtcDateTime.Now();
}