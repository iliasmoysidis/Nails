using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Professional
{
    public int Id { get; private set; }

    public FullName FullName { get; private set; }
    public Email Email { get; }
    public Phone Phone { get; private set; }
    public TaxIdentificationNumber TaxIdNumber { get; }
    public bool IsDeleted { get; private set; }
    public UtcDateTime? DeletedAt { get; private set; }

    public Professional(
        FullName fullName,
        Email email,
        Phone phone,
        TaxIdentificationNumber taxIdNumber)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        TaxIdNumber = taxIdNumber;
        IsDeleted = false;
        DeletedAt = null;
    }

    public void UpdatePersonalInfo(
        FullName? fullName = null,
        Phone? phone = null)
    {
        EnsureActive();

        if (fullName != null && fullName != FullName)
            FullName = fullName;

        if (phone != null && phone != Phone)
            Phone = phone;
    }

    public void Delete(IClock clock)
    {
        EnsureActive();

        IsDeleted = true;
        DeletedAt = clock.Now;
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new InvariantException("Professional is deleted.");
    }
}
