using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Offering : HistoricEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public Duration Duration { get; private set; } = null!;
    public string? Description { get; private set; }

    private Offering() { }

    public static Offering Create(int storeId, string name, Money price, Duration duration, IClock clock, string? description = null)
    {
        ValidateServiceInfo(name, description);

        var offering = new Offering
        {
            StoreId = storeId,
            Name = name.Trim(),
            Price = price,
            Duration = duration,
            Description = description?.Trim()
        };

        offering.MarkAsCreated(clock);

        return offering;
    }

    public void UpdateDetails(IClock clock, string? name = null, Money? price = null, Duration? duration = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot update inactive service.");
        }

        var hasChanges = false;

        if (name != null && name != Name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Service name cannot be empty.");
            }

            if (name.Length > 200)
            {
                throw new DomainException("Service name cannot exceed 200 characters.");
            }

            Name = name.Trim();
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

    public void Deactivate(IClock clock)
    {
        if (IsDeleted)
        {
            throw new DomainException("Service is already deactivated.");
        }

        SoftDelete(clock);
    }

    public static void ValidateServiceInfo(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Service name cannot be empty.");
        }

        if (name.Length > 200)
        {
            throw new DomainException("Service name cannot exceed 200 characters.");
        }

        if (description != null && description.Length > 500)
        {
            throw new DomainException("Description cannot exceed 500 characters.");
        }
    }
}
