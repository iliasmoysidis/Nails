using Application.Schedule.Common.Queries;

namespace Application.Schedule.GetWeeklySchedule;

public sealed class GetWeeklyScheduleHandler
{
    private readonly IProfessionalScheduleQueries _queries;

    public GetWeeklyScheduleHandler(IProfessionalScheduleQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffWorkingDayDTO>> Handle(GetWeeklyScheduleQuery query, CancellationToken ct)
    {
        return await _queries.GetWeeklyScheduleAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}