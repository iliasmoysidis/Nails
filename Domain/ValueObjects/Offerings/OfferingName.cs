using Domain.ValueObjects.Common;

namespace Domain.ValueObjects.Offerings;

public sealed record OfferingName : BaseName
{
    private const int MaxLength = 200;

    private OfferingName(string value) : base(value, MaxLength, "Service") { }

    public static OfferingName Create(string value)
        => new(value);
}