namespace Application.Features.StaffCalendars.GetExceptions;

public sealed record Query(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);