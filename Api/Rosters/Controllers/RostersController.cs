using Api.Rosters.Requests;
using Application.Rosters.AddOwner;
using Application.Rosters.GetProfessionalStores;
using Application.Rosters.GetStoreStaff;
using Application.Rosters.Hire;
using Application.Rosters.Terminate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rosters.Controllers;

[ApiController]
[Route("rosters")]
public sealed class RostersController : ControllerBase
{
    private readonly ISender _sender;

    public RostersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{storeId:int}/staff")]
    public async Task<IActionResult> GetStoreStaffAsync(
        int storeId,
        [FromQuery] GetStoreStaffRequest request,
        CancellationToken ct
    )
    {
        var query = new GetStoreStaffQuery(storeId, request.Page, request.Limit);

        var staff = await _sender.Send(query, ct);

        return Ok(staff);
    }

    [HttpGet("professionals/{professionalId:int}/stores")]
    public async Task<IActionResult> GetProfessionalStoresAsync(
        int professionalId,
        CancellationToken ct
    )
    {
        var stores = await _sender.Send(new GetProfessionalStoresQuery(professionalId), ct);

        return Ok(stores);
    }

    [HttpPost("{storeId:int}/owners")]
    public async Task<IActionResult> AddOwnerAsync(
        int storeId,
        AddStoreOwnerRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new AddStoreOwnerCommand(storeId, request.ProfessionalId), ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/staff")]
    public async Task<IActionResult> HireAsync(
        int storeId,
        HireStaffRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new HireStaffCommand(storeId, request.ProfessionalId), ct);

        return NoContent();
    }

    [HttpDelete("{storeId:int}/staff/{professionalId:int}")]
    public async Task<IActionResult> TerminateAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    )
    {
        await _sender.Send(new TerminateStaffCommand(storeId, professionalId), ct);

        return NoContent();
    }
}
