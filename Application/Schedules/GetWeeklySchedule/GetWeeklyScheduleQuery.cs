using MediatR;

namespace Application.Schedules.GetWeeklySchedule;

public sealed record GetWeeklyScheduleQuery(int StoreId, int ProfessionalId) : IRequest<IReadOnlyCollection<StaffWorkingDayDTO>>;