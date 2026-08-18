using MediatR;

namespace Application.Appointments.GetDetails;

public sealed class GetAppointmentDetailsHandler
    : IRequestHandler<GetAppointmentDetailsQuery, AppointmentDetailsDTO>
{
    private readonly GetAppointmentDetailsContext _context;

    public GetAppointmentDetailsHandler(GetAppointmentDetailsContext context)
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
