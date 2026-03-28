using Application.Abstractions.Queries;
using Application.DTO.StaffCalendar;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffWeeklyScheduleHandler
{
    private readonly IStaffCalendarQueries _queries;

    public GetStaffWeeklyScheduleHandler(IStaffCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffWorkingDayDTO>> Handle(GetStaffWeeklyScheduleQuery query, CancellationToken ct)
    {
        return await _queries.GetWeeklyScheduleAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}