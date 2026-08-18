using Application.Catalogs.Common.DTO;
using MediatR;

namespace Application.Catalogs.GetDetails;

public sealed record GetOfferingDetailsQuery(int OfferingId) : IRequest<OfferingDTO>;