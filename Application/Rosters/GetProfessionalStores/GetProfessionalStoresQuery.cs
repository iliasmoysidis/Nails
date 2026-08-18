using MediatR;

namespace Application.Rosters.GetProfessionalStores;

public sealed record GetProfessionalStoresQuery(int ProfessionalId) : IRequest<IReadOnlyCollection<ProfessionalStoreDTO>>;