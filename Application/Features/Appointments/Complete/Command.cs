using MediatR;

namespace Application.Features.Appointments.Complete;

public sealed record Command(int AppointmentId) : IRequest;