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
    public Phone Phone { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;

    private Professional()
    { }

    public static Professional Create(FirstName firstName, LastName lastName, Email email, Phone phone, string taxIdNumber, IClock clock)
    {
        ValidatePersonalInfo(taxIdNumber);

        var professional = new Professional
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            TaxIdNumber = taxIdNumber.Trim()
        };

        professional.MarkAsCreated(clock);

        return professional;
    }

    public void UpdatePersonalInfo(IClock clock, FirstName? firstName = null, LastName? lastName = null, Phone? phone = null)
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
            Phone = phone;
            hasChanges = true;
        }

        if (hasChanges) MarkAsUpdated(clock);
    }

    public string FullName => $"{FirstName} {LastName}";

    private static void ValidatePersonalInfo(string taxIdNumber)
    {
        ValidateTaxIdNumber(taxIdNumber);
    }

    private static void ValidateTaxIdNumber(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new DomainException("Tax ID number not found.");

        if (id.Length > 50) throw new DomainException("Tax ID number cannot exceed 50 characters.");
    }
}