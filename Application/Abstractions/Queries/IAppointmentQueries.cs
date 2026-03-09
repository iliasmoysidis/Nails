using Application.DTO.Appointment;

namespace Application.Abstractions.Queries;

public interface IAppointmentQueries
{
    Task<AppointmentDetailsDTO?> GetAppointmentDetailsAsync(int appointmentId, CancellationToken ct);
}