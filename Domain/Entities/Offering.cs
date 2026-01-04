using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Entities;

public class Offering : HistoricEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }
    public string? Description { get; private set; }

    private Offering() { }

    public static Offering Create(int storeId, string name, decimal price, TimeSpan duration, IClock clock, string? description = null)
    {
        ValidateServiceInfo(name, price, duration, description);

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

    public void UpdateDetails(IClock clock, string? name = null, decimal? price = null, TimeSpan? duration = null)
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

        if (price.HasValue && price.Value != Price)
        {
            if (price.Value <= 0)
            {
                throw new DomainException("Price must be greater than zero.");
            }

            if (price.Value > 10000)
            {
                throw new DomainException("Price cannot exceed 10,000.");
            }

            Price = price.Value;
            hasChanges = true;
        }

        if (duration.HasValue && duration.Value != Duration)
        {
            if (duration.Value <= TimeSpan.Zero)
            {
                throw new DomainException("Duration must be greater than zero.");
            }

            if (duration.Value > TimeSpan.FromHours(8))
            {
                throw new DomainException("Duration cannot exceed 8 hours.");
            }

            if (duration.Value.TotalMinutes % 15 != 0)
            {
                throw new DomainException("Duration must be in 15-minute increments.");
            }

            Duration = duration.Value;
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

    public static void ValidateServiceInfo(string name, decimal price, TimeSpan duration, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Service name cannot be empty.");
        }

        if (name.Length > 200)
        {
            throw new DomainException("Service name cannot exceed 200 characters.");
        }

        if (price <= 0)
        {
            throw new DomainException("Price must be greater than zero.");
        }

        if (price > 10000)
        {
            throw new DomainException("Price cannot exceed 10,000.");
        }

        if (duration <= TimeSpan.Zero)
        {
            throw new DomainException("Duration must be greater than zero.");
        }

        if (duration > TimeSpan.FromHours(8))
        {
            throw new DomainException("Duration cannot exceed 8 hours.");
        }

        if (duration.TotalMinutes % 15 != 0)
        {
            throw new DomainException("Duration must be in 15-minute increments.");
        }

        if (description != null && description.Length > 500)
        {
            throw new DomainException("Description cannot exceed 500 characters.");
        }
    }
}
