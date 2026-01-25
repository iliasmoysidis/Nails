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
    public OfferingName Name { get; private set; }
    public Money Price { get; private set; }
    public Duration Duration { get; private set; }
    public Description Description { get; private set; }

    private Offering(
        int storeId,
        OfferingName name,
        Money price,
        Duration duration,
        Description description
        )
    {
        StoreId = storeId;
        Name = name;
        Price = price;
        Duration = duration;
        Description = description;
    }

    public static Offering Create(
        int storeId,
        OfferingName name,
        Money price,
        Duration duration,
        Description description,
        IClock clock
        )
    {
        var offering = new Offering(
            storeId: storeId,
            name: name,
            price: price,
            duration: duration,
            description: description
        );

        offering.MarkAsCreated(clock);

        return offering;
    }

    public void UpdateDetails(IClock clock, OfferingName? name = null, Money? price = null, Duration? duration = null, Description? description = null)
    {
        EnsureNotDeleted();

        var changed = false;

        changed |= TryUpdateName(name);
        changed |= TryUpdatePrice(price);
        changed |= TryUpdateDuration(duration);
        changed |= TryUpdateDescription(description);

        if (changed) MarkAsUpdated(clock);
    }

    private bool TryUpdateName(OfferingName? name)
    {
        if (name is null || name == Name) return false;
        Name = name;
        return true;
    }

    private bool TryUpdatePrice(Money? price)
    {
        if (price is null) return false;

        if (price.Currency != Price.Currency)
            throw new InvariantException("Cannot change offering currency.");

        if (price == Price) return false;

        Price = price;
        return true;
    }

    private bool TryUpdateDuration(Duration? duration)
    {
        if (duration is null || duration == Duration) return false;
        Duration = duration;
        return true;
    }

    private bool TryUpdateDescription(Description? description)
    {
        if (description is null || description == Description) return false;
        Description = description;
        return true;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new StateException("Offering is deleted.");
    }
}
