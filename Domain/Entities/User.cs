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
    public Phone Phone { get; private set; } = null!;

    private User() { }

    public static User Create(FirstName firstName, LastName lastName, Email email, Phone phone, IClock clock)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone
        };

        user.MarkAsCreated(clock);
        return user;
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
}
