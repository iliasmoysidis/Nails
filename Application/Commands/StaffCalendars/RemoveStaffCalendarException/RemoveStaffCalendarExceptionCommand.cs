using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed record RemoveStaffCalendarExceptionCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;