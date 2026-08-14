using MediatR;

namespace Application.Roster.AddOwner;

public sealed record AddStoreOwnerCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;