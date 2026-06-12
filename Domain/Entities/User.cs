using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }
    public bool IsDeleted { get; private set; }
    public UtcDateTime? DeletedAt { get; private set; }

    private User(
        FullName fullName,
        Email email,
        Phone phone
    )
    {
        FullName = fullName;
        Email = email;
        Phone = phone;

        IsDeleted = false;
        DeletedAt = null;
    }

    public static User Create(FullName fullName, Email email, Phone phone)
    {
        var user = new User(fullName, email, phone);
        return user;
    }

    public void UpdatePersonalInfo(FullName? fullName = null, Phone? phone = null)
    {
        EnsureActive();
        TryUpdateName(fullName);
        TryUpdatePhone(phone);
    }

    public void Delete(IClock clock)
    {
        EnsureActive();

        IsDeleted = true;
        DeletedAt = clock.Now;
    }

    private void TryUpdateName(FullName? fullName)
    {
        if (fullName is null || fullName == FullName) return;
        FullName = fullName;
    }

    private void TryUpdatePhone(Phone? phone)
    {
        if (phone is null || phone == Phone) return;
        Phone = phone;
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new InvariantException("User is deleted.");
    }
}
