using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;
    public TaxIdentificationNumber TaxIdNumber { get; private set; } = null!;

    private Professional()
    { }

    public static Professional Create(FullName fullName, Email email, Phone phone, TaxIdentificationNumber taxIdNumber, IClock clock)
    {
        var professional = new Professional
        {
            FullName = fullName,
            Email = email,
            Phone = phone,
            TaxIdNumber = taxIdNumber
        };

        professional.MarkAsCreated(clock);

        return professional;
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