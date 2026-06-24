
using Domain.Catalog.ValueObjects;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;

namespace Domain.Catalog.Entities;

public class Offering
{
    public int Id { get; private set; }
    public int StoreId { get; }

    public OfferingName Name { get; private set; }
    public Money Price { get; private set; }
    public Duration Duration { get; private set; }
    public Description Description { get; private set; }

    public Offering(
        int storeId,
        OfferingName name,
        Money price,
        Duration duration,
        Description description)
    {
        StoreId = storeId;
        Name = name;
        Price = price;
        Duration = duration;
        Description = description;
    }

    public void UpdateDetails(
        OfferingName? name = null,
        Money? price = null,
        Duration? duration = null,
        Description? description = null)
    {
        if (name != null && name != Name)
            Name = name;

        if (price != null)
        {
            if (price.Currency != Price.Currency)
                throw new InvariantException("Cannot change offering currency.");

            if (price != Price)
                Price = price;
        }

        if (duration != null && duration != Duration)
            Duration = duration;

        if (description != null && description != Description)
            Description = description;
    }
}
