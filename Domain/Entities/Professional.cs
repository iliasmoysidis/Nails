using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;

    private readonly List<StoreOwner> _ownedStores = new();
    public IReadOnlyCollection<StoreOwner> OwnedStores => _ownedStores.AsReadOnly();

    private readonly List<StoreProfessional> _workplaces = new();
    public IReadOnlyCollection<StoreProfessional> Workplaces => _workplaces.AsReadOnly();

    private readonly List<ProfessionalService> _providedServices = new();
    public IReadOnlyCollection<ProfessionalService> ProvidedServices => _providedServices.AsReadOnly();

    private Professional() { }

    public static Professional Create(string name, string surname, string email, string phone, string taxIdNumber)
    {
        ValidatePersonalInfo(name, surname, email, phone, taxIdNumber);

        var professional = new Professional
        {
            Name = name.Trim(),
            Surname = surname.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone.Trim(),
            TaxIdNumber = taxIdNumber.Trim()
        };

        return professional;
    }

    public void UpdatePersonalInfo(string? name = null, string? surname = null, string? phone = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot update inactive professional.");
        }

        var hasChanges = false;
        if (name != null && name != Name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Name cannot be empty.");
            }

            if (name.Length > 100)
            {
                throw new DomainException("Name cannot exceed 100 characters.");
            }

            Name = name.Trim();
            hasChanges = true;
        }

        if (surname != null && surname != Surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new DomainException("Surname cannot be empty.");
            }

            if (surname.Length > 100)
            {
                throw new DomainException("Surname cannot exceed 100 characters.");
            }

            Surname = surname.Trim();
            hasChanges = true;
        }

        if (phone != null && phone != Phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new DomainException("Phone cannot be empty.");
            }

            if (phone.Length > 20)
            {
                throw new DomainException("Phone cannot exceed 20 characters.");
            }

            Phone = phone.Trim();
            hasChanges = true;
        }

        if (hasChanges) MarkAsUpdated();
    }

    public bool WorksAtStore(int storeId)
    {
        return !IsDeleted && _workplaces.Any(w => w.StoreId == storeId);
    }

    public bool IsOwnerOf(int storeId)
    {
        return _ownedStores.Any(m => m.StoreId == storeId && m.ProfessionalId == Id);
    }

    public string FullName => $"{Name} {Surname}";

    public void Deactivate()
    {
        if (IsDeleted)
        {
            throw new DomainException("Professional is already deactivated.");
        }

        SoftDelete();
    }

    private static void ValidatePersonalInfo(string name, string surname, string email, string phone, string taxIdNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new DomainException("Surname is required.");
        }

        if (surname.Length > 100)
        {
            throw new DomainException("Surname cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (!IsValidEmail(email))
        {
            throw new DomainException("Invalid email format.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new DomainException("Phone is required.");
        }

        if (phone.Length > 20)
        {
            throw new DomainException("Phone cannot exceed 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(taxIdNumber))
        {
            throw new DomainException("Tax ID number is required.");
        }

        if (taxIdNumber.Length > 50)
        {
            throw new DomainException("Tax ID number cannot exceed 50 characters.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}