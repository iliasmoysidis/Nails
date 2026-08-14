namespace Application.Roster.GetStoreStaff;

public sealed record GetStoreStaffQuery(int StoreId, int? Page, int? Limit);
