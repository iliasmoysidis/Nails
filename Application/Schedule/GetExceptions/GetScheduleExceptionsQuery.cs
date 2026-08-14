namespace Application.Schedule.GetExceptions;

public sealed record GetScheduleExceptionsQuery(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
