namespace Infrastructure.Roster;

public class StaffEntity
{
    public int StoreId { get; set; }
    public List<StaffMemberEntity> Members { get; set; } = new();
}