using Application.Common.DTO;
using Application.Schedules.Common.Queries;
using MediatR;

namespace Application.Schedules.GetExceptions;

public sealed class GetScheduleExceptionsHandler
    : IRequestHandler<GetScheduleExceptionsQuery, PagedResult<StaffCalendarExceptionDTO>>
{
    private readonly IProfessionalScheduleQueries _queries;

    public GetScheduleExceptionsHandler(IProfessionalScheduleQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StaffCalendarExceptionDTO>> Handle(
        GetScheduleExceptionsQuery query,
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
