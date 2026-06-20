using Domain.Assignments;
using Domain.Stores;
using Domain.Common.Exceptions;

namespace Domain.Catalog.Services;

public sealed class StoreOfferingRemoval
{
    private readonly Store _store;
    private readonly AssignmentRegistry _assignments;
    private readonly StoreCatalog _catalog;

    public StoreOfferingRemoval(Store store, AssignmentRegistry assignments, StoreCatalog catalog)
    {
        ValidateComposition(store, assignments, catalog);

        _store = store;
        _assignments = assignments;
        _catalog = catalog;
    }

    public void RemoveOffering(int offeringId)
    {
        _store.EnsureOpen();
        _assignments.RemoveByOffering(offeringId);
        _catalog.RemoveOffering(offeringId);
    }

    private static void ValidateComposition(Store store, AssignmentRegistry assignments, StoreCatalog catalog)
    {
        if (store.Id != catalog.StoreId)
            throw new InvariantException("Catalog does not belong to store.");

        if (store.Id != assignments.StoreId)
            throw new InvariantException("Assignments do not belong to the store.");
    }
}
