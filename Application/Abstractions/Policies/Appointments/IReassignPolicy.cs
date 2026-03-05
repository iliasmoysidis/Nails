using Application.Commands.Appointments;
using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface IReassignPolicy
{
    void EnsureCanReassign(Staff staff, int professionalId);
}