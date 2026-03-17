using Domain.ValueObjects.Identity;

namespace Domain.Entities;

public class Professional
{
    public int Id { get; private set; }

    public FullName FullName { get; private set; }
    public Email Email { get; }
    public Phone Phone { get; private set; }
    public TaxIdentificationNumber TaxIdNumber { get; }

    private Professional(
        FullName fullName,
        Email email,
        Phone phone,
        TaxIdentificationNumber taxIdNumber)
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
        TaxIdentificationNumber taxIdNumber)
        => new(fullName, email, phone, taxIdNumber);

    public void UpdatePersonalInfo(
        FullName? fullName = null,
        Phone? phone = null)
    {
        if (fullName != null && fullName != FullName)
            FullName = fullName;

        if (phone != null && phone != Phone)
            Phone = phone;
    }
}