using Application.Abstractions.Queries;
using Application.DTO.StaffCalendar;

namespace Application.Features.StaffCalendars.GetExceptions;

public sealed class Handler
{
    private readonly IStaffCalendarQueries _queries;

    public Handler(IStaffCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffCalendarExceptionDTO>> Handle(
        Query query,
        CancellationToken ct
    )
    {
        return await _queries.GetExceptionsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}