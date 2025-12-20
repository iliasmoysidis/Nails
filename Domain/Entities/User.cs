using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Entities;

public class User : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private User() { }

    public static User Create(string name, string surname, string email, string phone, IClock clock)
    {
        ValidatePersonalInfo(name, surname, email, phone);

        var user = new User
        {
            Name = name.Trim(),
            Surname = surname.Trim(),
            Email = email.Trim(),
            Phone = phone.Trim()
        };

        user.MarkAsCreated(clock);
        return user;
    }

    public void Deactivate(IClock clock)
    {
        if (IsDeleted) throw new DomainException("User is already deactivated.");

        SoftDelete(clock);
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

    private static void ValidatePersonalInfo(string name, string surname, string email, string phone)
    {
        ValidateName(name);
        ValidateSurname(surname);
        ValidateEmail(email);
        ValidatePhone(phone);
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
}
