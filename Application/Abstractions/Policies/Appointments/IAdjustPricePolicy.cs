using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface IAdjustPricePolicy
{
    void EnsureCanAdjustPrice(Staff staff);
}