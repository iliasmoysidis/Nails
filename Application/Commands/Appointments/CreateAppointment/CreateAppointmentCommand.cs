using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Commands.Appointments;

public sealed record CreateAppointmentCommand(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    UtcDateTime StartAt,
    string? Notes
) : IRequest<int>;

