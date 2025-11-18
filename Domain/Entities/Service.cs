using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Service : HistoricEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }

    public Store Store { get; private set; } = null!;

    private Service() { }

    public static Service Create(Store store, string name, decimal price, TimeSpan duration)
    {
        ValidateServiceInfo(name, price, duration);

        if (store.IsDeleted)
        {
            throw new DomainException("Cannot create service for inactive store.");
        }

        return new Service
        {
            StoreId = store.Id,
            Name = name.Trim(),
            Price = price,
            Duration = duration,
            Store = store,
        };
    }

    public void UpdateDetails(string? name = null, decimal? price = null, TimeSpan? duration = null)
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
            MarkAsUpdated();
        }
    }

    public void Deactivate()
    {
        if (IsDeleted)
        {
            throw new DomainException("Service is already deactivated.");
        }

        SoftDelete();
    }

    public static void ValidateServiceInfo(string name, decimal price, TimeSpan duration)
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
    }
}
