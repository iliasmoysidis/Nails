using MediatR;

namespace Application.Features.Appointments.Confirm;

public sealed record Command(int AppointmentId) : IRequest;
