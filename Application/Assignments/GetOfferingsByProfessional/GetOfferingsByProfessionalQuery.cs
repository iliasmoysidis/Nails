using Application.Common.DTO;
using MediatR;

namespace Application.Assignments.GetOfferingsByProfessional;

public sealed record GetOfferingsByProfessionalQuery(
    int StoreId,
    int ProfessionalId,
    int? Page,
    int? Limit
) : IRequest<PagedResult<OfferingSummaryDTO>>;
