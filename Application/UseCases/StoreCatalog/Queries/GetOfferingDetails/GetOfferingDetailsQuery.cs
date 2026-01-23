using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCatalog.Queries.GetOfferingDetails;

public sealed record GetOfferingDetailsQuery(int OfferingId)
    : IQuery<OfferingDetailsDTO?>;