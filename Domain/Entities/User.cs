using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public bool IsCustomer { get; private set; } = true;
    public bool IsProfessional { get; private set; } = false;
}
