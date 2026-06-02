using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Services;

public class StoreAssignments
{
    public Staff Staff { get; }
    public StoreCatalog StoreCatalog { get; }
    public Assignments Assignments { get; }

    public StoreAssignments(
        Staff staff,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        if (staff.StoreId != storeCatalog.StoreId)
            throw new InvariantException("Store staff and catalog mismatch.");

        if (assignments.StoreId != staff.StoreId)
            throw new InvariantException("Store assignments mismatch.");

        Staff = staff;
        StoreCatalog = storeCatalog;
        Assignments = assignments;
    }

    public void Assign(int professionalId, IReadOnlyCollection<int> offeringIds)
    {
        EnsureProfessionalWorksForStore(professionalId);

        foreach (var offeringId in offeringIds)
        {
            EnsureStoreOffersService(offeringId);
            Assignments.Add(professionalId, offeringId);
        }
    }

    public void Remove(int professionalId, IReadOnlyCollection<int> offeringIds)
    {
        EnsureProfessionalWorksForStore(professionalId);

        foreach (var offeringId in offeringIds)
        {
            Assignments.Remove(professionalId, offeringId);
        }
    }

    private void EnsureProfessionalWorksForStore(int professionalId)
    {
        if (!Staff.IsStaff(professionalId))
            throw new InvariantException("Professional does not work for the store.");
    }

    private void EnsureStoreOffersService(int offeringId)
    {
        StoreCatalog.GetOffering(offeringId);
    }
}
