using Application.DTO.Appointment;

namespace Application.Features.Appointments.GetDetails;

public sealed class Context
{
    public AppointmentDetailsDTO? Appointment { get; set; }
}