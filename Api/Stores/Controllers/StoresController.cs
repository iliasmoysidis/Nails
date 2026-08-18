using Api.Stores.Requests;
using Application.Stores.Close;
using Application.Stores.Create;
using Application.Stores.GetAll;
using Application.Stores.GetDetails;
using Application.Stores.Search;
using Application.Stores.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Stores.Controllers;

[ApiController]
[Route("stores")]
public sealed class StoresController : ControllerBase
{
    private readonly ISender _sender;

    public StoresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        CreateStoreRequest request,
        CancellationToken ct
    )
    {
        var command = new CreateStoreCommand(
            request.ProfessionalId,
            request.Name,
            request.Street,
            request.City,
            request.PostalCode,
            request.State,
            request.CountryCode,
            request.Email,
            request.PhoneCountryCode,
            request.PhoneNumber,
            request.TaxCountryCode,
            request.TaxNumber
        );

        var storeId = await _sender.Send(command, ct);

        return Created($"/stores/{storeId}", new { id = storeId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllStoresRequest request,
        CancellationToken ct
    )
    {
        var stores = await _sender.Send(new GetAllStoresQuery(request.Page, request.Limit), ct);

        return Ok(stores);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] SearchStoresRequest request,
        CancellationToken ct
    )
    {
        var query = new SearchStoresQuery(
            request.Name,
            request.City,
            request.CountryCode,
            request.Page,
            request.Limit
        );

        var stores = await _sender.Send(query, ct);

        return Ok(stores);
    }

    [HttpGet("{storeId:int}")]
    public async Task<IActionResult> GetDetailsAsync(
        int storeId,
        CancellationToken ct
    )
    {
        var details = await _sender.Send(new GetStoreDetailsQuery(storeId), ct);

        return Ok(details);
    }

    [HttpPatch("{storeId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int storeId,
        UpdateStoreRequest request,
        CancellationToken ct
    )
    {
        var command = new UpdateStoreCommand(
            storeId,
            request.Name,
            request.Street,
            request.City,
            request.PostalCode,
            request.State,
            request.CountryCode,
            request.PhoneCountryCode,
            request.PhoneNumber
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpPost("{storeId:int}/close")]
    public async Task<IActionResult> CloseAsync(
        int storeId,
        CancellationToken ct
    )
    {
        await _sender.Send(new CloseStoreCommand(storeId), ct);

        return NoContent();
    }
}
