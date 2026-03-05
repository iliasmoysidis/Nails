using Domain.Entities;

namespace Application.Abstractions.Policies.ProfessionalOfferings;

public interface IUnassignOfferingPolicy
{
    void EnsureCanUnassignOffering(int professionalId, Staff staff);
}