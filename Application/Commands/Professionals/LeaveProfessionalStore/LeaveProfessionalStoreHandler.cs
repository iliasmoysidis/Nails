using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreHandler
    : IRequestHandler<LeaveProfessionalStoreCommand>
{
    private readonly LeaveProfessionalStoreContext _ctx;
    private readonly IProfessionalExitService _service;
    private readonly IStaffCalendarRepository _calendarRepo;
    private readonly IClock _clock;

    public LeaveProfessionalStoreHandler(
        LeaveProfessionalStoreContext ctx,
        IProfessionalExitService service,
        IStaffCalendarRepository calendarRepo,
        IClock clock)
    {
        _ctx = ctx;
        _service = service;
        _calendarRepo = calendarRepo;
        _clock = clock;
    }

    public async Task Handle(
        LeaveProfessionalStoreCommand command,
        CancellationToken ct)
    {
        try
        {
            _service.LeaveStore(
                _ctx.Staff,
                _ctx.Assignments,
                _ctx.UpcomingAppointments,
                command.ProfessionalId,
                _clock
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }

        await _calendarRepo.RemoveProfessionalAsync(
            command.StoreId,
            command.ProfessionalId,
            ct
        );
    }
}