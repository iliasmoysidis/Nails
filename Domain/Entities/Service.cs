using Domain.Common;

namespace Domain.Entities;

public class Service : BaseEntity
{
    public int StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }
}
