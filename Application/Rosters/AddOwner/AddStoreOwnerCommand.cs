using MediatR;

namespace Application.Rosters.AddOwner;

public sealed record AddStoreOwnerCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;