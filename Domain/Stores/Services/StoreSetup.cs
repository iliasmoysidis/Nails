using Domain.Catalog;
using Domain.Roster;
using Domain.Assignments;
using Domain.Calendar;

namespace Domain.Stores.Services;

public sealed class StoreSetup
{
    public Staff Staff { get; }
    public StoreCatalog StoreCatalog { get; }
    public AssignmentRegistry Assignments { get; }
    public StoreCalendar StoreCalendar { get; }

    public StoreSetup(
        int storeId,
        int ownerProfessionalId
    )
    {
        Staff = Staff.Create(storeId, ownerProfessionalId);
        StoreCatalog = new StoreCatalog(storeId);
        Assignments = new AssignmentRegistry(storeId);
        StoreCalendar = new StoreCalendar(storeId);
    }
}
