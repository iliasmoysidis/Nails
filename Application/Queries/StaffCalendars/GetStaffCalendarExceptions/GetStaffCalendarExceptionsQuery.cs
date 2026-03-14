namespace Application.Queries.StaffCalendars;

public sealed record GetStaffCalendarExceptionsQuery(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);