using Domain.ValueObjects.Common;

namespace Domain.ValueObjects.Store;

public sealed record StoreName : BaseName
{
    private const int MaxLength = 100;

    private StoreName(string value) : base(value, MaxLength, "Store") { }

    public static StoreName Create(string value)
        => new(value);
}