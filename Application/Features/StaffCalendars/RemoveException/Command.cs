using MediatR;

namespace Application.Features.StaffCalendars.RemoveException;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;