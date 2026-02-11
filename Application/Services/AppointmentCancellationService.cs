using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Interfaces;

namespace Application.Services;

public sealed class AppointmentCancellationService : IAppointmentCancellationService
{
    private readonly IAppointmentRepository _repo;

    public AppointmentCancellationService(IAppointmentRepository repo)
    {
        _repo = repo;
    }

    public async Task CancelUpcomingForStoreAsync(int storeId, IClock clock, CancellationToken ct)
    {
        var upcoming = await _repo.GetUpcomingByStoreIdAsync(storeId, ct);

        foreach (var appointment in upcoming)
        {
            appointment.Cancel(clock, "Store has been closed.");
        }
    }
}