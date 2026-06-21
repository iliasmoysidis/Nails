using Application.Calendar.Common.DTO;
using Application.Calendar.Common.Queries;
using Application.Calendar;

namespace Application.Calendar.GetProfessionalCalendar;

public sealed class Handler
{
    private readonly IStoreCalendarQueries _queries;

    public Handler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> Handle(
        Query query,
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