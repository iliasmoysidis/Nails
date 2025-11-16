using System.ComponentModel.DataAnnotations;
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
    private readonly List<ProfessionalService> _providers = new();
    public IReadOnlyCollection<ProfessionalService> Providers => _providers.AsReadOnly();

    private Service() { }

    public static Service Create(Store store, string name, decimal price, TimeSpan duration)
    {
        ValidateServiceInfo(name, price, duration);

        if (!store.IsActive)
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
        if (!IsActive)
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
                throw new DomainException("Duration must be in 15-minute icrements.");
            }

            Duration = duration.Value;
            hasChanges = true;
        }

        if (hasChanges)
        {
            MarkAsUpdated();
        }
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (!IsActive)
        {
            throw new DomainException("Cannot update price for inactive service.");
        }

        if (newPrice <= 0)
        {
            throw new DomainException("Price must be greater than zero.");
        }

        if (newPrice > 10000)
        {
            throw new DomainException("Price cannot exceed 10,000.");
        }

        Price = newPrice;
        MarkAsUpdated();
    }

    public void UpdateDuration(TimeSpan newDuration)
    {
        if (!IsActive)
        {
            throw new DomainException("Cannot update duration for inactive service.");
        }

        if (newDuration <= TimeSpan.Zero)
        {
            throw new DomainException("Duration must be greater than zero.");
        }

        if (newDuration > TimeSpan.FromHours(8))
        {
            throw new DomainException("Duration cannot exceed 8 hours.");
        }

        if (newDuration.TotalMinutes % 15 != 0)
        {
            throw new DomainException("Duration must be in 15-minute icrements.");
        }

        Duration = newDuration;
        MarkAsUpdated();
    }

    public void AddProvider(Professional professional)
    {
        if (!IsActive)
        {
            throw new DomainException("Cannot add provider to inactive service.");
        }

        if (!professional.IsActive)
        {
            throw new DomainException("Cannot add inactive professional as provider.");
        }

        if (!professional.WorksAtStore(StoreId))
        {
            throw new DomainException("Professional must work at this store to provide this service.");
        }

        var alreadyProvider = _providers.Any(p => p.ProfessionalId == professional.Id);
        if (alreadyProvider)
        {
            throw new DomainException("Professional is already a provider of this service.");
        }

        var professionalService = ProfessionalService.Create(professional.Id, Id);
        _providers.Add(professionalService);

        MarkAsUpdated();
    }

    public void RemoveProvider(int professionalId)
    {
        var provider = _providers.FirstOrDefault(p => p.ProfessionalId == professionalId);

        if (provider == null)
        {
            throw new DomainException("Professional is not a provider of this service.");
        }

        if (_providers.Count == 1)
        {
            throw new DomainException("Cannot remove the last provider. Service must have at least one provider.");
        }

        _providers.Remove(provider);
        MarkAsUpdated();
    }

    public bool HasProvider(int professionalId)
    {
        return _providers.Any(p => p.ProfessionalId == professionalId);
    }

    public bool HasProviders()
    {
        return _providers.Any();
    }

    public int GetProviderCount()
    {
        return _providers.Count();
    }

    public IEnumerable<int> GetProviderIds()
    {
        return _providers.Select(p => p.ProfessionalId);
    }

    public bool IsAvailableWith(int professionalId)
    {
        return IsActive && HasProvider(professionalId);
    }

    public void Deactivate(string? reason = null)
    {
        if (!IsActive)
        {
            throw new DomainException("Service is already deactivated.");
        }

        SoftDelete();
        MarkAsUpdated();
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
            throw new DomainException("Duration must be in 15-minute icrements.");
        }
    }
}
