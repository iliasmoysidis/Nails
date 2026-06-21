namespace Application.Calendar.GetProfessionalCalendar;

public sealed record Query(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);