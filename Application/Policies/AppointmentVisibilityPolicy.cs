using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;

namespace Application.Policies;

public sealed class AppointmentVisibilityPolicy : IAppointmentVisibilityPolicy
{
    private readonly IStaffRepository _repo;

    public AppointmentVisibilityPolicy(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureCanViewAsync(Appointment appointment, int agentId, CancellationToken ct)
    {
        if (appointment.UserId == agentId)
            return;

        if (appointment.ProfessionalId == agentId)
            return;

        var staff = await _repo.GetStaffAsync(appointment.StoreId, ct);

        if (staff.IsOwner(agentId))
            return;

        throw new ApplicationLayerException("You are not allowed to view the details of this appointment.");
    }
}