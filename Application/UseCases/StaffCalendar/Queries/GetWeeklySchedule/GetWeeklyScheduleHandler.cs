using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.StaffCalendar.Queries.GetWeeklySchedule;

public sealed class GetWeeklyScheduleHandler
    : IQueryHandler<GetWeeklyScheduleQuery, IReadOnlyCollection<DailyScheduleDTO>>
{
    private readonly IStaffCalendarRepository _repo;

    public GetWeeklyScheduleHandler(IStaffCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<DailyScheduleDTO>> Handle(
        GetWeeklyScheduleQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found.");

        var result = new List<DailyScheduleDTO>();

        for (int i = 0; i < 7; i++)
        {
            var date = query.WeekStart.AddDays(i);

            result.Add(new DailyScheduleDTO(
                date.DayOfWeek,
                calendar.GetWorkingTimeRanges(date)
            ));
        }

        return result;
    }
}