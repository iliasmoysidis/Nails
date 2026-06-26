using Domain.Common.ValueObjects;

namespace Domain.Stores.ValueObjects;

public sealed record StoreName : BaseName
{
    public const int MaxLength = 100;

    private StoreName(string value) : base(value, MaxLength, "Store") { }

    public static StoreName Create(string value)
        => new(value);
}
