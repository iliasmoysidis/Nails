using MediatR;

namespace Application.Schedule.AddVacation;

public sealed record AddVacationCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;