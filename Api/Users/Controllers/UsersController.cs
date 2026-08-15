using Api.Users.Requests;
using Application.Users.Delete;
using Application.Users.GetDetails;
using Application.Users.Register;
using Application.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken ct
    )
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneCountryCode,
            request.PhoneNumber
        );

        var userId = await _sender.Send(command, ct);

        return Created($"/users/{userId}", new { id = userId });
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetDetailsAsync(
        int userId,
        CancellationToken ct
    )
    {
        var details = await _sender.Send(new GetUserDetailsQuery(userId), ct);

        return Ok(details);
    }

    [HttpPatch("{userId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int userId,
        UpdateUserRequest request,
        CancellationToken ct
    )
    {
        var command = new UpdateUserCommand(
            userId,
            request.FirstName,
            request.LastName,
            request.PhoneCountryCode,
            request.PhoneNumber
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int userId,
        CancellationToken ct
    )
    {
        await _sender.Send(new DeleteUserCommand(userId), ct);

        return NoContent();
    }
}
