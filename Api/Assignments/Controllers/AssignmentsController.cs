using Api.Assignments.Requests;
using Application.Assignments.Add;
using Application.Assignments.GetOfferingsByProfessional;
using Application.Assignments.GetProfessionalsByOffering;
using Application.Assignments.Remove;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Assignments.Controllers;

[ApiController]
[Route("assignments")]
public sealed class AssignmentsController : ControllerBase
{
    private readonly ISender _sender;

    public AssignmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(
        AddAssignmentsRequest request,
        CancellationToken ct
    )
    {
        var command = new AddAssignmentsCommand(
            request.StoreId,
            request.ProfessionalId,
            request.OfferingIds
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(
        RemoveAssignmentsRequest request,
        CancellationToken ct
    )
    {
        var command = new RemoveAssignmentsCommand(
            request.StoreId,
            request.ProfessionalId,
            request.OfferingIds
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpGet("professionals/{professionalId:int}/offerings")]
    public async Task<IActionResult> GetOfferingsByProfessionalAsync(
        int professionalId,
        [FromQuery] GetOfferingsByProfessionalRequest request,
        CancellationToken ct
    )
    {
        var query = new GetOfferingsByProfessionalQuery(
            request.StoreId,
            professionalId,
            request.Page,
            request.Limit
        );

        var offerings = await _sender.Send(query, ct);

        return Ok(offerings);
    }

    [HttpGet("offerings/{offeringId:int}/professionals")]
    public async Task<IActionResult> GetProfessionalsByOfferingAsync(
        int offeringId,
        [FromQuery] GetProfessionalsByOfferingRequest request,
        CancellationToken ct
    )
    {
        var query = new GetProfessionalsByOfferingQuery(
            request.StoreId,
            offeringId,
            request.Page,
            request.Limit
        );

        var professionals = await _sender.Send(query, ct);

        return Ok(professionals);
    }
}
