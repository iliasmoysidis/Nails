using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;

    private Professional()
    { }

    public static Professional Create(FirstName firstName, LastName lastName, Email email, string phone, string taxIdNumber, IClock clock)
    {
        ValidatePersonalInfo(phone, taxIdNumber);

        var professional = new Professional
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone.Trim(),
            TaxIdNumber = taxIdNumber.Trim()
        };

        professional.MarkAsCreated(clock);

        return professional;
    }

    public void UpdatePersonalInfo(IClock clock, FirstName? firstName = null, LastName? lastName = null, string? phone = null)
    {
        if (IsDeleted) throw new DomainException("Cannot modify a deactivated user.");

        var hasChanges = false;

        if (firstName is not null && firstName != FirstName)
        {
            FirstName = firstName;
            hasChanges = true;
        }

        if (lastName is not null && lastName != LastName)
        {
            LastName = lastName;
            hasChanges = true;
        }

        if (phone != null && phone != Phone)
        {
            ValidatePhone(phone);

            Phone = phone.Trim();
            hasChanges = true;
        }

        if (hasChanges) MarkAsUpdated(clock);
    }

    public string FullName => $"{FirstName} {LastName}";

    private static void ValidatePersonalInfo(string phone, string taxIdNumber)
    {
        ValidatePhone(phone);
        ValidateTaxIdNumber(taxIdNumber);
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) throw new DomainException("Phone is required.");

        if (phone.Length > 20) throw new DomainException("Phone cannot exceed 20 characters.");
    }

    private static void ValidateTaxIdNumber(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new DomainException("Tax ID number not found.");

        if (id.Length > 50) throw new DomainException("Tax ID number cannot exceed 50 characters.");
    }
}