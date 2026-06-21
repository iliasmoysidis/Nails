using MediatR;

namespace Application.Roster.AddOwner;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;