using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Services;

public sealed class StoreAssignments
{
    private readonly Store _store;
    private readonly Staff _staff;
    private readonly StoreCatalog _storeCatalog;
    private readonly Assignments _assignments;

    public StoreAssignments(
        Store store,
        Staff staff,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        ValidateComposition(
            store,
            staff,
            storeCatalog,
            assignments
        );

        _store = store;
        _staff = staff;
        _storeCatalog = storeCatalog;
        _assignments = assignments;
    }

    public void Assign(int professionalId, IReadOnlyCollection<int> offeringIds)
    {
        _store.EnsureOpen();
        EnsureProfessionalWorksForStore(professionalId);

        foreach (var offeringId in offeringIds)
        {
            EnsureOfferingExists(offeringId);
            _assignments.Add(professionalId, offeringId);
        }
    }

    public void Remove(int professionalId, IReadOnlyCollection<int> offeringIds)
    {
        _store.EnsureOpen();
        EnsureProfessionalWorksForStore(professionalId);

        foreach (var offeringId in offeringIds)
        {
            _assignments.Remove(professionalId, offeringId);
        }
    }

    private void EnsureProfessionalWorksForStore(int professionalId)
    {
        if (!_staff.IsStaff(professionalId))
            throw new InvariantException("Professional does not work for the store.");
    }

    private void EnsureOfferingExists(int offeringId)
    {
        _storeCatalog.GetOffering(offeringId);
    }

    private void ValidateComposition(
        Store store,
        Staff staff,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        if (staff.StoreId != store.Id)
            throw new InvariantException("Staff does not belong to this store.");

        if (storeCatalog.StoreId != store.Id)
            throw new InvariantException("Store catalog does not belong to this store.");

        if (assignments.StoreId != store.Id)
            throw new InvariantException("Assignments do not belong to this store.");
    }
}
