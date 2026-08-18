using Application.Schedules.Common.Queries;
using MediatR;

namespace Application.Schedules.GetWeeklySchedule;

public sealed class GetWeeklyScheduleHandler
    : IRequestHandler<GetWeeklyScheduleQuery, IReadOnlyCollection<StaffWorkingDayDTO>>
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