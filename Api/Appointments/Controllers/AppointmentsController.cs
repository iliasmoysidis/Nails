using Api.Appointments.Requests;
using Application.Appointments.AdjustPrice;
using Application.Appointments.Cancel;
using Application.Appointments.Complete;
using Application.Appointments.Confirm;
using Application.Appointments.Create;
using Application.Appointments.GetDetails;
using Application.Appointments.GetProfessionalAppointments;
using Application.Appointments.GetStoreAppointments;
using Application.Appointments.GetUserAppointments;
using Application.Appointments.MarkNoShow;
using Domain.Common.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Appointments.Controllers;

[ApiController]
[Route("appointments")]
public sealed class AppointmentsController : ControllerBase
{
    private readonly ISender _sender;

    public AppointmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        CreateAppointmentRequest request,
        CancellationToken ct
    )
    {
        var command = new CreateAppointmentCommand(
            request.UserId,
            request.ProfessionalId,
            request.OfferingId,
            request.StoreId,
            UtcDateTime.FromUtc(request.StartAt),
            request.Notes
        );

        var appointmentId = await _sender.Send(command, ct);

        return Created($"/appointments/{appointmentId}", new { id = appointmentId });
    }

    [HttpGet("{appointmentId:int}")]
    public async Task<IActionResult> GetDetailsAsync(
        int appointmentId,
        CancellationToken ct
    )
    {
        var details = await _sender.Send(new GetAppointmentDetailsQuery(appointmentId), ct);

        return Ok(details);
    }

    [HttpPost("{appointmentId:int}/cancel")]
    public async Task<IActionResult> CancelAsync(
        int appointmentId,
        CancelAppointmentRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new CancelAppointmentCommand(appointmentId, request.Reason), ct);

        return NoContent();
    }

    [HttpPost("{appointmentId:int}/confirm")]
    public async Task<IActionResult> ConfirmAsync(
        int appointmentId,
        CancellationToken ct
    )
    {
        await _sender.Send(new ConfirmAppointmentCommand(appointmentId), ct);

        return NoContent();
    }

    [HttpPost("{appointmentId:int}/complete")]
    public async Task<IActionResult> CompleteAsync(
        int appointmentId,
        CancellationToken ct
    )
    {
        await _sender.Send(new CompleteAppointmentCommand(appointmentId), ct);

        return NoContent();
    }

    [HttpPost("{appointmentId:int}/no-show")]
    public async Task<IActionResult> MarkNoShowAsync(
        int appointmentId,
        CancellationToken ct
    )
    {
        await _sender.Send(new MarkAppointmentNoShowCommand(appointmentId), ct);

        return NoContent();
    }

    [HttpPost("{appointmentId:int}/price")]
    public async Task<IActionResult> AdjustPriceAsync(
        int appointmentId,
        AdjustAppointmentPriceRequest request,
        CancellationToken ct
    )
    {
        var command = new AdjustAppointmentPriceCommand(
            appointmentId,
            request.Amount,
            request.Currency,
            request.Reason
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpGet("professionals/{professionalId:int}")]
    public async Task<IActionResult> GetByProfessionalAsync(
        int professionalId,
        [FromQuery] GetProfessionalAppointmentsRequest request,
        CancellationToken ct
    )
    {
        var query = new GetProfessionalAppointmentsQuery(
            professionalId,
            request.From,
            request.To,
            request.Page,
            request.Limit
        );

        var appointments = await _sender.Send(query, ct);

        return Ok(appointments);
    }

    [HttpGet("stores/{storeId:int}")]
    public async Task<IActionResult> GetByStoreAsync(
        int storeId,
        [FromQuery] GetStoreAppointmentsRequest request,
        CancellationToken ct
    )
    {
        var query = new GetStoreAppointmentsQuery(
            storeId,
            request.From,
            request.To,
            request.Page,
            request.Limit
        );

        var appointments = await _sender.Send(query, ct);

        return Ok(appointments);
    }

    [HttpGet("users/{userId:int}")]
    public async Task<IActionResult> GetByUserAsync(
        int userId,
        [FromQuery] GetUserAppointmentsRequest request,
        CancellationToken ct
    )
    {
        var query = new GetUserAppointmentsQuery(
            userId,
            request.From,
            request.To,
            request.Page,
            request.Limit
        );

        var appointments = await _sender.Send(query, ct);

        return Ok(appointments);
    }
}
