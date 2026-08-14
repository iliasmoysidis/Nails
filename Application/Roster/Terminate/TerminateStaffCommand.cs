using MediatR;

namespace Application.Roster.Terminate;

public sealed record TerminateStaffCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;
