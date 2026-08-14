using Domain.Common.ValueObjects;
using MediatR;

namespace Application.Appointments.Create;

public sealed record CreateAppointmentCommand(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    UtcDateTime StartAt,
    string? Notes
) : IRequest<int>;

