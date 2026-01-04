using Domain.ValueObjects.Common;

namespace Domain.ValueObjects.Identity;

public sealed record LastName : BaseName
{
    private const int MaxLength = 100;

    private LastName(string value) : base(value, MaxLength, "Last") { }

    public static LastName Create(string value)
        => new(value);
}