using MediatR;

namespace Application.Rosters.Terminate;

public sealed record TerminateStaffCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;
