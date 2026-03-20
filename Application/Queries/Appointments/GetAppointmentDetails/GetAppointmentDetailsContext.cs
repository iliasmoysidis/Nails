using Application.DTO.Appointment;

namespace Application.Queries.Appointments;

public sealed class GetAppointmentDetailsContext
{
    public AppointmentDetailsDTO? Appointment { get; set; }
}