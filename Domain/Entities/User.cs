using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class User : HistoricEntity
{
    public int Id { get; private set; }
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private User() { }

    public static User Create(FirstName firstName, LastName lastName, Email email, string phone, IClock clock)
    {
        ValidatePersonalInfo(phone);

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone.Trim()
        };

        user.MarkAsCreated(clock);
        return user;
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

    private static void ValidatePersonalInfo(string phone)
    {
        ValidatePhone(phone);
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) throw new DomainException("Phone is required.");

        if (phone.Length > 20) throw new DomainException("Phone cannot exceed 20 characters.");
    }
}
