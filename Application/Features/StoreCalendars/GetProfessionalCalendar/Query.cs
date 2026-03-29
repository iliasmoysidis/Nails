namespace Application.Features.StoreCalendars.GetProfessionalCalendar;

public sealed record Query(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);