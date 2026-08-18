using Api.Catalogs.Requests;
using Application.Catalogs.Create;
using Application.Catalogs.GetDetails;
using Application.Catalogs.GetStoreOfferings;
using Application.Catalogs.Remove;
using Application.Catalogs.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Catalogs.Controllers;

[ApiController]
[Route("catalogs")]
public sealed class CatalogsController : ControllerBase
{
    private readonly ISender _sender;

    public CatalogsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{storeId:int}/offerings")]
    public async Task<IActionResult> CreateAsync(
        int storeId,
        CreateOfferingRequest request,
        CancellationToken ct
    )
    {
        var command = new CreateOfferingCommand(
            storeId,
            request.Name,
            request.Price,
            request.Currency,
            request.DurationMinutes,
            request.Description
        );

        var offeringId = await _sender.Send(command, ct);

        return Created($"/catalogs/offerings/{offeringId}", new { id = offeringId });
    }

    [HttpGet("{storeId:int}/offerings")]
    public async Task<IActionResult> GetStoreOfferingsAsync(
        int storeId,
        CancellationToken ct
    )
    {
        var offerings = await _sender.Send(new GetStoreOfferingsQuery(storeId), ct);

        return Ok(offerings);
    }

    [HttpGet("offerings/{offeringId:int}")]
    public async Task<IActionResult> GetDetailsAsync(
        int offeringId,
        CancellationToken ct
    )
    {
        var details = await _sender.Send(new GetOfferingDetailsQuery(offeringId), ct);

        return Ok(details);
    }

    [HttpPatch("{storeId:int}/offerings/{offeringId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int storeId,
        int offeringId,
        UpdateOfferingRequest request,
        CancellationToken ct
    )
    {
        var command = new UpdateOfferingCommand(
            storeId,
            offeringId,
            request.Name,
            request.Price,
            request.DurationMinutes,
            request.Description
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{storeId:int}/offerings/{offeringId:int}")]
    public async Task<IActionResult> RemoveAsync(
        int storeId,
        int offeringId,
        CancellationToken ct
    )
    {
        await _sender.Send(new RemoveOfferingCommand(storeId, offeringId), ct);

        return NoContent();
    }
}
