using Domain.Entities;

namespace Application.Abstractions.Policies.ProfessionalOfferings;

public interface IAssignOfferingPolicy
{
    void EnsureCanAssignOffering(Staff staff);
}