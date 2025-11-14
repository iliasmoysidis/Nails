using Domain.Enums;

namespace Domain.Entities;

public class StoreManager
{
    public int StoreId { get; private set; }
    public int UserId { get; private set; }
    public StoreRole Role { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public Store Store { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private StoreManager()
    {
        StartDate = DateTime.UtcNow;
    }
}
