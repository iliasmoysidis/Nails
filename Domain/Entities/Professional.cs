using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }
    public TaxIdentificationNumber TaxIdNumber { get; private set; }

    private Professional(
        FullName fullName,
        Email email,
        Phone phone,
        TaxIdentificationNumber taxIdNumber
    )
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        TaxIdNumber = taxIdNumber;
    }

    public static Professional Create(
        FullName fullName,
        Email email,
        Phone phone,
        TaxIdentificationNumber taxIdNumber,
        IClock clock
        )
    {
        var professional = new Professional(
            fullName: fullName,
            email: email,
            phone: phone,
            taxIdNumber: taxIdNumber
        );

        professional.MarkAsCreated(clock);

        return professional;
    }

    public void UpdatePersonalInfo(IClock clock, FullName? fullName = null, Phone? phone = null)
    {
        EnsureActive();

        var changed = false;

        changed |= TryUpdateFullName(fullName);
        changed |= TryUpdatePhone(phone);

        if (changed) MarkAsUpdated(clock);
    }

    private bool TryUpdateFullName(FullName? fullName)
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
}