using Application.Common.DTO;
using MediatR;

namespace Application.Assignments.GetProfessionalsByOffering;

public sealed record GetProfessionalsByOfferingQuery(
    int StoreId,
    int OfferingId,
    int? Page,
    int? Limit
) : IRequest<PagedResult<ProfessionalSummaryDTO>>;
