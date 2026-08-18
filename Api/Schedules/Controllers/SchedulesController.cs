using Api.Schedules.Requests;
using Application.Schedules.AddSpecialAvailability;
using Application.Schedules.AddVacation;
using Application.Schedules.GetExceptions;
using Application.Schedules.GetWeeklySchedule;
using Application.Schedules.RemoveException;
using Application.Schedules.SetDayOff;
using Application.Schedules.SetWorkingDay;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Schedules.Controllers;

[ApiController]
[Route("schedules")]
public sealed class SchedulesController : ControllerBase
{
    private readonly ISender _sender;

    public SchedulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{storeId:int}/professionals/{professionalId:int}/weekly")]
    public async Task<IActionResult> GetWeeklyScheduleAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    )
    {
        var schedule = await _sender.Send(new GetWeeklyScheduleQuery(storeId, professionalId), ct);

        return Ok(schedule);
    }

    [HttpGet("{storeId:int}/professionals/{professionalId:int}/exceptions")]
    public async Task<IActionResult> GetExceptionsAsync(
        int storeId,
        int professionalId,
        [FromQuery] GetScheduleExceptionsRequest request,
        CancellationToken ct
    )
    {
        var query = new GetScheduleExceptionsQuery(
            storeId,
            professionalId,
            request.From,
            request.To,
            request.Page,
            request.Limit
        );

        var exceptions = await _sender.Send(query, ct);

        return Ok(exceptions);
    }

    [HttpPost("{storeId:int}/professionals/{professionalId:int}/special-availability")]
    public async Task<IActionResult> AddSpecialAvailabilityAsync(
        int storeId,
        int professionalId,
        AddSpecialAvailabilityRequest request,
        CancellationToken ct
    )
    {
        var command = new AddSpecialAvailabilityCommand(
            storeId,
            professionalId,
            request.Date,
            request.TimeRanges
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/professionals/{professionalId:int}/vacation")]
    public async Task<IActionResult> AddVacationAsync(
        int storeId,
        int professionalId,
        AddVacationRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new AddVacationCommand(storeId, professionalId, request.Date), ct);

        return NoContent();
    }

    [HttpDelete("{storeId:int}/professionals/{professionalId:int}/exceptions/{date}")]
    public async Task<IActionResult> RemoveExceptionAsync(
        int storeId,
        int professionalId,
        DateOnly date,
        CancellationToken ct
    )
    {
        await _sender.Send(new RemoveScheduleExceptionCommand(storeId, professionalId, date), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/professionals/{professionalId:int}/days-off")]
    public async Task<IActionResult> SetDayOffAsync(
        int storeId,
        int professionalId,
        SetScheduleDayOffRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new SetScheduleDayOffCommand(storeId, professionalId, request.Day), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/professionals/{professionalId:int}/working-days")]
    public async Task<IActionResult> SetWorkingDayAsync(
        int storeId,
        int professionalId,
        SetScheduleWorkingDayRequest request,
        CancellationToken ct
    )
    {
        var command = new SetScheduleWorkingDayCommand(
            storeId,
            professionalId,
            request.Day,
            request.TimeRanges
        );

        await _sender.Send(command, ct);

        return NoContent();
    }
}
