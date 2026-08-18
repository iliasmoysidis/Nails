using MediatR;

namespace Application.Professionals.Search;

public sealed record SearchProfessionalsQuery(
    string? Name,
    int? OfferingId,
    string? City,
    int? StoreId
) : IRequest<IReadOnlyCollection<ProfessionalSearchResultDTO>>;