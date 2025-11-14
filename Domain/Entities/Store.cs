using Domain.Common;

namespace Domain.Entities;

public class Store : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
}
