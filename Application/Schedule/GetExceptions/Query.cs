namespace Application.Schedule.GetExceptions;

public sealed record Query(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);