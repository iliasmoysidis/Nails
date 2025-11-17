using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class User : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private User() { }

    public static User Create(string name, string surname, string email, string phone)
    {
        ValidatePersonalInfo(name, surname, email, phone);

        return new User
        {
            Name = name,
            Surname = surname,
            Email = email,
            Phone = phone
        };
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

    private static void ValidatePersonalInfo(string name, string surname, string email, string phone)
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
