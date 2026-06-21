using MediatR;

namespace Application.Roster.Hire;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;
