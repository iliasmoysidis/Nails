using Domain.Roster.EnumObjects;

namespace Infrastructure.Roster;

public class StaffRoleEntity
{
    public int StoreId { get; set; }
    public int ProfessionalId { get; set; }
    public StaffRole Role { get; set; }
}