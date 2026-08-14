using MediatR;

namespace Application.Roster.Hire;

public sealed record HireStaffCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;
