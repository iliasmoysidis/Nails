using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;

    private Professional() { }

    public static Professional Create(string name, string surname, string email, string phone, string taxIdNumber)
    {
        ValidatePersonalInfo(name, surname, email, phone, taxIdNumber);

        return new Professional
        {
            Name = name.Trim(),
            Surname = surname.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone.Trim(),
            TaxIdNumber = taxIdNumber.Trim()
        };
    }

    public void UpdatePersonalInfo(IClock clock, string? name = null, string? surname = null, string? phone = null)
    {
        if (IsDeleted) throw new DomainException("Cannot modify a deactivated user.");

        var hasChanges = false;

        if (name != null && name != Name)
        {
            ValidateName(name);

            Name = name.Trim();
            hasChanges = true;
        }

        if (surname != null && surname != Surname)
        {
            ValidateSurname(surname);

            Surname = surname.Trim();
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

    public string FullName => $"{Name} {Surname}";

    public void Deactivate(IClock clock)
    {
        if (IsDeleted) throw new DomainException("Professional is already deactivated.");

        SoftDelete(clock);
    }

    private static void ValidatePersonalInfo(string name, string surname, string email, string phone, string taxIdNumber)
    {
        ValidateName(name);
        ValidateSurname(surname);
        ValidatePhone(phone);
        ValidateEmail(email);
        ValidateTaxIdNumber(taxIdNumber);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required.");

        if (name.Length > 100) throw new DomainException("Name cannot exceed 100 characters.");
    }

    private static void ValidateSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname)) throw new DomainException("Surname is required.");

        if (surname.Length > 100) throw new DomainException("Surname cannot exceed 100 characters.");
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) throw new DomainException("Phone is required.");

        if (phone.Length > 20) throw new DomainException("Phone cannot exceed 20 characters.");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");

        try
        {
            _ = new System.Net.Mail.MailAddress(email);
        }
        catch
        {
            throw new DomainException("Invalid email.");
        }
    }

    private static void ValidateTaxIdNumber(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new DomainException("Tax ID number not found.");

        if (id.Length > 50) throw new DomainException("Tax ID number cannot exceed 50 characters.");
    }
}