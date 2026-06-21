using MediatR;

namespace Application.Roster.Terminate;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;
