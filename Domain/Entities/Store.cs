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
    public string Address { get; private set; } = null!;
    public TaxIdentificationNumber TaxIdNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;

    private Store() { }

    public static Store Create(StoreName name, string address, TaxIdentificationNumber taxIdNumber, Email email, Phone phone, IClock clock)
    {
        ValidateStoreInfo(address);

        var store = new Store
        {
            Name = name,
            Address = address,
            TaxIdNumber = taxIdNumber,
            Email = email,
            Phone = phone
        };

        store.MarkAsCreated(clock);

        return store;
    }

    private static void ValidateStoreInfo(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new DomainException("Address is required.");
        }

        if (address.Length > 200)
        {
            throw new DomainException("Address cannot exceed 200 characters.");
        }
    }
}
