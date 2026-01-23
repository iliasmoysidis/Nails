using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCatalog.Queries.GetProfessionalOfferings;

public sealed record GetProfessionalOfferingsQuery(int StoreId, int ProfessionalId)
    : IQuery<IReadOnlyCollection<StoreOfferingDTO>>;