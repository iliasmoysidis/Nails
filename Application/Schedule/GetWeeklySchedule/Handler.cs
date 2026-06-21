using Application.Schedule.Common.Queries;

namespace Application.Schedule.GetWeeklySchedule;

public sealed class Handler
{
    private readonly IProfessionalScheduleQueries _queries;

    public Handler(IProfessionalScheduleQueries queries)
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