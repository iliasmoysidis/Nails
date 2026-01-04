using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Offering : HistoricEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public OfferingName Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public Duration Duration { get; private set; } = null!;
    public string? Description { get; private set; }

    private Offering() { }

    public static Offering Create(int storeId, OfferingName name, Money price, Duration duration, IClock clock, string? description = null)
    {
        ValidateServiceInfo(description);

        var offering = new Offering
        {
            StoreId = storeId,
            Name = name,
            Price = price,
            Duration = duration,
            Description = description?.Trim()
        };

        offering.MarkAsCreated(clock);

        return offering;
    }

    public void UpdateDetails(IClock clock, OfferingName? name = null, Money? price = null, Duration? duration = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot update inactive service.");
        }

        var hasChanges = false;

        if (name is not null && name != Name)
        {
            Name = name;
            hasChanges = true;
        }

        if (price is not null && price != Price)
        {
            Price = price;
            hasChanges = true;
        }

        if (duration is not null && duration != Duration)
        {
            Duration = duration;
            hasChanges = true;
        }

        if (hasChanges)
        {
            MarkAsUpdated(clock);
        }
    }

    public static void ValidateServiceInfo(string? description = null)
    {
        if (description != null && description.Length > 500)
        {
            throw new DomainException("Description cannot exceed 500 characters.");
        }
    }
}
