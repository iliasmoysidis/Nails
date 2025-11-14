using System;
using Domain.Enums;

namespace Domain.Entities;

public class StoreManager
{
    public int StoreId { get; private set; }
    public int UserId { get; private set; }
    public StoreRole Role { get; private set; }
    public DateTime StartDate { get; private set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; private set; }
}
