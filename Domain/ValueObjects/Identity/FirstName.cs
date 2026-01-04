using Domain.ValueObjects.Common;

namespace Domain.ValueObjects.Identity;

public sealed record FirstName : BaseName
{
    private const int MaxLength = 100;

    private FirstName(string value) : base(value, MaxLength, "First") { }

    public static FirstName Create(string value)
        => new(value);
}