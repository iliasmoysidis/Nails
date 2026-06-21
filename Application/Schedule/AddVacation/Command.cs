using MediatR;

namespace Application.Schedule.AddVacation;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;