
using Application.DTO.Appointment;

namespace Application.Queries.Appointments;

public sealed class GetAppointmentDetailsHandler
{
    private readonly GetAppointmentDetailsContext _context;

    public GetAppointmentDetailsHandler(
        GetAppointmentDetailsContext context
    )
    {
        _context = context;
    }

    public Task<AppointmentDetailsDTO> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        var appointment = _context.Appointment
            ?? throw new InvalidOperationException("Appointment context not loaded.");

        return Task.FromResult(appointment);
    }
}