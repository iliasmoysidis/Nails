using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCatalog.Queries.GetCatalogOverview;

public sealed record GetCatalogOverviewQuery(int StoreId)
    : IQuery<IReadOnlyCollection<OfferingOverviewDTO>>;