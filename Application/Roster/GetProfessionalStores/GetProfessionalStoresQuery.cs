using MediatR;

namespace Application.Roster.GetProfessionalStores;

public sealed record GetProfessionalStoresQuery(int ProfessionalId) : IRequest<IReadOnlyCollection<ProfessionalStoreDTO>>;