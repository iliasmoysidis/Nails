namespace Application.Roster.GetStoreStaff;

public sealed record Query(int StoreId, int? Page, int? Limit);
