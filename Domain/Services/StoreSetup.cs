using Domain.Entities;

namespace Domain.Services;

public sealed class StoreSetup
{
    public Staff Staff { get; }
    public StoreCatalog StoreCatalog { get; }
    public Assignments Assignments { get; }
    public StoreCalendar StoreCalendar { get; }

    public StoreSetup(
        int storeId,
        int ownerProfessionalId
    )
    {
        Staff = Staff.Create(storeId, ownerProfessionalId);
        StoreCatalog = StoreCatalog.Create(storeId);
        Assignments = Assignments.Create(storeId);
        StoreCalendar = StoreCalendar.Create(storeId);
    }
}
