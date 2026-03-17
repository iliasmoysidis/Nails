using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentAuthorizer
    : IAuthorizer<CreateAppointmentCommand>
{
    private readonly AuthorizationGuard _auth;

    public CreateAppointmentAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(CreateAppointmentCommand request, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(request.UserId);

        return Task.CompletedTask;
    }
}