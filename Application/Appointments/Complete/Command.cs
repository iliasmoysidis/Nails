using MediatR;

namespace Application.Appointments.Complete;

public sealed record Command(int AppointmentId) : IRequest;