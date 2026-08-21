using Domain.Common;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;
using Domain.Stores.Events;
using Domain.Stores.ValueObjects;

namespace Domain.Stores;

public class Store : Entity
{
    public int Id { get; private set; }

    public StoreName Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public TaxIdentificationNumber TaxIdNumber { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;

    public bool IsClosed { get; private set; }
    public UtcDateTime? ClosedAt { get; private set; }

    private Store() { }

    public Store(
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
