using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class ProfessionalDeletion
{
    private readonly Professional _professional;
    private readonly IReadOnlyCollection<Store> _ownedStores;
    private readonly IReadOnlyCollection<EmploymentTermination> _employments;

    public ProfessionalDeletion(
        Professional professional,
        IReadOnlyCollection<Store> ownedStores,
        IReadOnlyCollection<EmploymentTermination> employments
    )
    {
        _professional = professional;
        _ownedStores = ownedStores;
        _employments = employments;
    }

    public void Delete(IClock clock)
    {
        EnsureDoesNotOwnStores();
        
        foreach (var employment in _employments)
        {
            employment.Terminate(clock);
        }

        _professional.Delete(clock);
    }

    private void EnsureDoesNotOwnStores()
    {
        if (_ownedStores.Count > 0)
            throw new InvariantException("Professional owns one or more stores.");
    }
}
