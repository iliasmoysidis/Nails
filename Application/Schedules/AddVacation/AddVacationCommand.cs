using MediatR;

namespace Application.Schedules.AddVacation;

public sealed record AddVacationCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;