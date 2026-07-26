using Application.Common.DTO;
using Application.Schedule.Common.Queries;

namespace Application.Schedule.GetExceptions;

public sealed class Handler
{
    private readonly IProfessionalScheduleQueries _queries;

    public Handler(IProfessionalScheduleQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StaffCalendarExceptionDTO>> Handle(
        Query query,
        CancellationToken ct
    )
    {
        return await _queries.GetExceptionsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
