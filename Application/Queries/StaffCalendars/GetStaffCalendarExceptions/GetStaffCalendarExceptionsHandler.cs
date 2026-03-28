using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.StaffCalendar;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffCalendarExceptionsHandler
{
    private readonly IStaffCalendarQueries _queries;

    public GetStaffCalendarExceptionsHandler(IStaffCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffCalendarExceptionDTO>> Handle(
        GetStaffCalendarExceptionsQuery query,
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