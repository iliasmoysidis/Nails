
using Application.Exceptions;
using Application.Policies.Interfaces;
using Application.Repositories;

namespace Application.Implementation.Policies;

public sealed class ProfessionalAppointmentsAccessPolicy : IProfessionalAppointmentsAccessPolicy
{
    private readonly IStaffRepository _repo;

    public ProfessionalAppointmentsAccessPolicy(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureCanViewAsync(int agentId, int storeId, int professionalId, CancellationToken ct)
    {
        if (agentId == professionalId)
            return;

        var staff = await _repo.GetStaffAsync(storeId, ct);

        if (staff.IsOwner(agentId))
            return;

        throw new ApplicationLayerException("You are not allowed to view this professional's appointments.");
    }
}