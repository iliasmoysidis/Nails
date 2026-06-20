using Domain.Common.ValueObjects;

namespace Domain.Common;

public interface IClock
{
    UtcDateTime Now { get; }
}