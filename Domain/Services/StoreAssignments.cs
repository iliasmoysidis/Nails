using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Services;

public sealed class StoreAssignments
{
    private readonly Professional _professional;
    private readonly Store _store;
    private readonly StoreCatalog _storeCatalog;
    private readonly Assignments _assignments;

    public StoreAssignments(
        Professional professional,
        Store store,
        Staff staff,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        ValidateComposition(
            professional,
            store,
            staff,
            storeCatalog,
            assignments
        );

        _professional = professional;
        _store = store;
        _storeCatalog = storeCatalog;
        _assignments = assignments;
    }

    public void Assign(IReadOnlyCollection<int> offeringIds)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();

        foreach (var offeringId in offeringIds)
        {
            EnsureOfferingExists(offeringId);
            _assignments.Add(_professional.Id, offeringId);
        }
    }

    public void Remove(IReadOnlyCollection<int> offeringIds)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();

        foreach (var offeringId in offeringIds)
        {
            _assignments.Remove(_professional.Id, offeringId);
        }
    }

    private void EnsureOfferingExists(int offeringId)
    {
        _storeCatalog.GetOffering(offeringId);
    }

    private void ValidateComposition(
        Professional professional,
        Store store,
        Staff staff,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        if (!staff.IsStaff(professional.Id))
            throw new InvariantException("Professional does not work for the store.");

        if (staff.StoreId != store.Id)
            throw new InvariantException("Staff does not belong to this store.");

        if (storeCatalog.StoreId != store.Id)
            throw new InvariantException("Store catalog does not belong to this store.");

        if (assignments.StoreId != store.Id)
            throw new InvariantException("Assignments do not belong to this store.");
    }
}
