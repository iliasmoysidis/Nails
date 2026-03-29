using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Features.Appointments.Create;

public sealed record Command(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    UtcDateTime StartAt,
    string? Notes
) : IRequest<int>;

