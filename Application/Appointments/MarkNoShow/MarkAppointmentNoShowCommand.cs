using MediatR;

namespace Application.Appointments.MarkNoShow;

public sealed record MarkAppointmentNoShowCommand(int AppointmentId) : IRequest;