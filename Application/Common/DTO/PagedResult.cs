namespace Application.Common.DTO;

public sealed record PaginationMetadata(
    int Page,
    int Limit,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount,
    PaginationMetadata? Pagination
);
