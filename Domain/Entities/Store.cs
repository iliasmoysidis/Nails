using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Store : Entity
{
    public int Id { get; private set; }

    public StoreName Name { get; private set; }
    public Address Address { get; private set; }
    public TaxIdentificationNumber TaxIdNumber { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }

    public bool IsClosed { get; private set; }
    public UtcDateTime? ClosedAt { get; private set; }

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
        IsClosed = false;
        ClosedAt = null;
    }

    public static Store Create(
        StoreName name,
        Address address,
        TaxIdentificationNumber taxIdNumber,
        Email email,
        Phone phone)
        => new(name, address, taxIdNumber, email, phone);

    public void Close(IClock clock)
    {
        EnsureOpen();

        IsClosed = true;
        ClosedAt = clock.Now;
        RaiseDomainEvent(new StoreClosedDomainEvent(Id, clock.Now));
    }

    public void UpdateDetails(
        StoreName? name = null,
        Address? address = null,
        Phone? phone = null)
    {
        EnsureOpen();

        if (name != null && name != Name)
            Name = name;

        if (address != null && address != Address)
            Address = address;

        if (phone != null && phone != Phone)
            Phone = phone;
    }

    public void EnsureOpen()
    {
        if (IsClosed)
            throw new InvariantException("Store is closed.");
    }
}
