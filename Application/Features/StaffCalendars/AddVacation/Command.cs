using MediatR;

namespace Application.Features.StaffCalendars.AddVacation;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;