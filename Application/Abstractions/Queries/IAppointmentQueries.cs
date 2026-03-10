using Application.DTO.Appointment;

namespace Application.Abstractions.Queries;

public interface IAppointmentQueries
{
    Task<AppointmentDetailsDTO?> GetAppointmentDetailsAsync(int appointmentId, CancellationToken ct);

    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetProfessionalAppointmentsAsync(
        int professionalId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetUserAppointmentsAsync(
        int userId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetStoreAppointmentsAsync(
        int storeId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );
}