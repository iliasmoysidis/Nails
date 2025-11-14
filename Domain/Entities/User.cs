using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private User() { }
}
