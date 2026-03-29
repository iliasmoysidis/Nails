using Application.Abstractions.Queries;
using Application.DTO.Calendar;

namespace Application.Features.StoreCalendars.GetProfessionalCalendar;

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