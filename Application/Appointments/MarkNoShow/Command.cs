using MediatR;

namespace Application.Appointments.MarkNoShow;

public sealed record Command(int AppointmentId) : IRequest;