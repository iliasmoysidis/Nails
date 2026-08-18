using MediatR;

namespace Application.Rosters.Hire;

public sealed record HireStaffCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;
