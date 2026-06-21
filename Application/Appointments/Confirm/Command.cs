using MediatR;

namespace Application.Appointments.Confirm;

public sealed record Command(int AppointmentId) : IRequest;
