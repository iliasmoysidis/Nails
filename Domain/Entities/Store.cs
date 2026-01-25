using Domain.Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;

namespace Domain.Entities;

public class Store : HistoricEntity
{
    public int Id { get; private set; }
    public StoreName Name { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public TaxIdentificationNumber TaxIdNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;

    private Store() { }

    private Store(
        StoreName name,
        Address address,
        TaxIdentificationNumber taxIdNumber,
        Email email,
        Phone phone
    )
    {
        Name = name;
        Address = address;
        TaxIdNumber = taxIdNumber;
        Email = email;
        Phone = phone;
    }

    public static Store Create(
        StoreName name,
        Address address,
        TaxIdentificationNumber taxIdNumber,
        Email email,
        Phone phone,
        IClock clock
        )
    {
        var store = new Store(
            name: name,
            address: address,
            taxIdNumber: taxIdNumber,
            email: email,
            phone: phone
        );


        store.MarkAsCreated(clock);

        return store;
    }

    public void UpdateDetails(
        IClock clock,
        StoreName? name = null,
        Address? address = null,
        Phone? phone = null
    )
    {
        EnsureNotDeleted();

        var changed = false;

        changed |= TryUpdateName(name);
        changed |= TryUpdateAddress(address);
        changed |= TryUpdatePhone(phone);

        if (changed)
            MarkAsUpdated(clock);
    }

    private bool TryUpdateName(StoreName? name)
    {
        if (name is null || name == Name) return false;
        Name = name;
        return true;
    }

    private bool TryUpdateAddress(Address? address)
    {
        if (address is null || address == Address) return false;
        Address = address;
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
            throw new DomainException("Store is deleted.");
    }
}
