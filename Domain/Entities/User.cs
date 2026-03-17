using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }

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

    public static User Create(FullName fullName, Email email, Phone phone)
    {
        var user = new User(fullName, email, phone);
        return user;
    }

    public void UpdatePersonalInfo(FullName? fullName = null, Phone? phone = null)
    {
        TryUpdateName(fullName);
        TryUpdatePhone(phone);
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
}
