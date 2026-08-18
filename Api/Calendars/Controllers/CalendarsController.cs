using Api.Calendars.Requests;
using Application.Calendars.AddHoliday;
using Application.Calendars.AddSpecialHours;
using Application.Calendars.GetAvailability;
using Application.Calendars.GetCalendar;
using Application.Calendars.RemoveException;
using Application.Calendars.SetDayOff;
using Application.Calendars.SetWorkingDay;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Calendars.Controllers;

[ApiController]
[Route("calendars")]
public sealed class CalendarsController : ControllerBase
{
    private readonly ISender _sender;

    public CalendarsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{storeId:int}")]
    public async Task<IActionResult> GetCalendarAsync(
        int storeId,
        [FromQuery] GetCalendarRequest request,
        CancellationToken ct
    )
    {
        var query = new GetCalendarQuery(storeId, request.From, request.To);

        var calendar = await _sender.Send(query, ct);

        return Ok(calendar);
    }

    [HttpGet("{storeId:int}/availability")]
    public async Task<IActionResult> GetAvailabilityAsync(
        int storeId,
        [FromQuery] GetCalendarAvailabilityRequest request,
        CancellationToken ct
    )
    {
        var query = new GetCalendarAvailabilityQuery(
            storeId,
            request.ProfessionalId,
            request.OfferingId,
            request.Date
        );

        var slots = await _sender.Send(query, ct);

        return Ok(slots);
    }

    [HttpPost("{storeId:int}/holidays")]
    public async Task<IActionResult> AddHolidayAsync(
        int storeId,
        AddHolidayRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new AddHolidayCommand(storeId, request.Date), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/special-hours")]
    public async Task<IActionResult> AddSpecialHoursAsync(
        int storeId,
        AddSpecialHoursRequest request,
        CancellationToken ct
    )
    {
        var command = new AddSpecialHoursCommand(storeId, request.Date, request.TimeRanges);

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{storeId:int}/exceptions/{date}")]
    public async Task<IActionResult> RemoveExceptionAsync(
        int storeId,
        DateOnly date,
        CancellationToken ct
    )
    {
        await _sender.Send(new RemoveCalendarExceptionCommand(storeId, date), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/days-off")]
    public async Task<IActionResult> SetDayOffAsync(
        int storeId,
        SetDayOffRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new SetCalendarDayOffCommand(storeId, request.Day), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/working-days")]
    public async Task<IActionResult> SetWorkingDayAsync(
        int storeId,
        SetWorkingDayRequest request,
        CancellationToken ct
    )
    {
        var command = new SetCalendarWorkingDayCommand(storeId, request.Day, request.TimeRanges);

        await _sender.Send(command, ct);

        return NoContent();
    }
}
