using Application.Abstractions.Queries;
using Application.DTO.Calendar;

namespace Application.Queries.StoreCalendars;

public sealed class GetStoreProfessionalCalendarHandler
{
    private readonly IStoreCalendarQueries _queries;

    public GetStoreProfessionalCalendarHandler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> Handle(
        GetStoreProfessionalCalendarQuery query,
        CancellationToken ct
    )
    {
        return await _queries.GetProfessionalCalendarAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}