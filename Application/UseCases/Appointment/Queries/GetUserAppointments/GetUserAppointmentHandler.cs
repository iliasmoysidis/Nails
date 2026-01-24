using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.Appointment.Queries.GetUserAppointments;

public sealed class GetUserAppointmentsHandler : IQueryHandler<GetUserAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IAppointmentReadRepository _repo;
    private readonly ICurrentUser _currentUser;

    public GetUserAppointmentsHandler(IAppointmentReadRepository repo, ICurrentUser currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        if (query.UserId != _currentUser.UserId)
            throw new ApplicationLayerException("You are not allowed to view these appointments.");

        return await _repo.GetForUserAsync(query.UserId, ct);
    }
}