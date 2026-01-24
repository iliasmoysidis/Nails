using Domain.Entities;
using Domain.Services.Appointment;

namespace Application.Repositories;

public interface IAppointmentWriteRepository
{
    Task<AppointmentContext> LoadContextAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<Appointment?> GetAppointmentAsync(int appointmentId, CancellationToken ct);

    void Add(Appointment appointment);
}