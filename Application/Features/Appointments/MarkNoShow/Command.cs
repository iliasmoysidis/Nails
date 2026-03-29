using MediatR;

namespace Application.Features.Appointments.MarkNoShow;

public sealed record Command(int AppointmentId) : IRequest;