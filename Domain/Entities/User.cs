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

    private User(
        FullName fullName,
        Email email,
        Phone phone
    )
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
    }

    public static User Create(FullName fullName, Email email, Phone phone, IClock clock)
    {
        var user = new User(fullName, email, phone);
        user.MarkAsCreated(clock);
        return user;
    }

    public void UpdatePersonalInfo(IClock clock, FullName? fullName = null, Phone? phone = null)
    {
        EnsureNotDeleted();

        var changed = false;

        changed |= TryUpdateName(fullName);
        changed |= TryUpdatePhone(phone);

        if (changed)
            MarkAsUpdated(clock);
    }

    private bool TryUpdateName(FullName? fullName)
    {
        if (fullName is null || fullName == FullName) return false;
        FullName = fullName;
        return true;
    }

    private bool TryUpdatePhone(Phone? phone)
    {
        if (phone is null || phone == Phone) return false;
        Phone = phone;
        return true;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Cannot modify a deactivated user.");
    }
}
