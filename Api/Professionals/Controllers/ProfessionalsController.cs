using Api.Professionals.Requests;
using Application.Professionals.Delete;
using Application.Professionals.GetDetails;
using Application.Professionals.Register;
using Application.Professionals.Search;
using Application.Professionals.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Professionals.Controllers;

[ApiController]
[Route("professionals")]
public sealed class ProfessionalsController : ControllerBase
{
    private readonly ISender _sender;

    public ProfessionalsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(
        RegisterProfessionalRequest request,
        CancellationToken ct
    )
    {
        var command = new RegisterProfessionalCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneCountryCode,
            request.PhoneNumber,
            request.TaxCountryCode,
            request.TaxIdNumber
        );

        var professionalId = await _sender.Send(command, ct);

        return Created($"/professionals/{professionalId}", new { id = professionalId });
    }

    [HttpGet("{professionalId:int}")]
    public async Task<IActionResult> GetDetailsAsync(
        int professionalId,
        CancellationToken ct
    )
    {
        var details = await _sender.Send(new GetProfessionalDetailsQuery(professionalId), ct);

        return Ok(details);
    }

    [HttpPatch("{professionalId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int professionalId,
        UpdateProfessionalRequest request,
        CancellationToken ct
    )
    {
        var command = new UpdateProfessionalCommand(
            professionalId,
            request.FirstName,
            request.LastName,
            request.PhoneCountryCode,
            request.PhoneNumber
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{professionalId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int professionalId,
        CancellationToken ct
    )
    {
        await _sender.Send(new DeleteProfessionalCommand(professionalId), ct);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] SearchProfessionalRequest request,
        CancellationToken ct
    )
    {
        var query = new SearchProfessionalsQuery(
            request.Name,
            request.Email,
            request.Phone,
            request.Page,
            request.Limit
        );

        var result = await _sender.Send(query, ct);

        return Ok(result);
    }
}
