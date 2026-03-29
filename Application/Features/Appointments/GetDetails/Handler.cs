
using Application.DTO.Appointment;

namespace Application.Features.Appointments.GetDetails;

public sealed class Handler
{
    private readonly Context _context;

    public Handler(
        Context context
    )
    {
        _context = context;
    }

    public Task<AppointmentDetailsDTO> Handle(Query query, CancellationToken ct)
    {
        var appointment = _context.Appointment
            ?? throw new InvalidOperationException("Appointment context not loaded.");

        return Task.FromResult(appointment);
    }
}