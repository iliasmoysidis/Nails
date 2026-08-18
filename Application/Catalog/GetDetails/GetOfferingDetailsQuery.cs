using Application.Catalog.Common.DTO;
using MediatR;

namespace Application.Catalog.GetDetails;

public sealed record GetOfferingDetailsQuery(int OfferingId) : IRequest<OfferingDTO>;