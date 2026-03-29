using Application.Abstractions.Queries;
using Application.DTO.StaffCalendar;

namespace Application.Features.StaffCalendars.GetWeeklySchedule;

public sealed class Handler
{
    private readonly IStaffCalendarQueries _queries;

    public Handler(IStaffCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffWorkingDayDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetWeeklyScheduleAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}