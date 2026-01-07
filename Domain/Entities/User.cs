using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class User : HistoricEntity
{
    public int Id { get; private set; }
    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;

    private User() { }

    public static User Create(FullName fullName, Email email, Phone phone, IClock clock)
    {
        var user = new User
        {
            FullName = fullName,
            Email = email,
            Phone = phone
        };

        user.MarkAsCreated(clock);
        return user;
    }

    public void UpdatePersonalInfo(IClock clock, FullName? fullName = null, Phone? phone = null)
    {
        if (IsDeleted) throw new DomainException("Cannot modify a deactivated user.");

        var hasChanges = false;

        if (fullName is not null && fullName != FullName)
        {
            FullName = fullName;
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
