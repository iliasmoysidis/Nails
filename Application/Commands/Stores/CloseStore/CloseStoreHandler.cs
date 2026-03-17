using Application.Abstractions.Repositories;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Stores;

public sealed class CloseStoreHandler
    : IRequestHandler<CloseStoreCommand>
{
    private readonly CloseStoreContext _ctx;
    private readonly IStoreClosureService _service;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IClock _clock;

    public CloseStoreHandler(
        CloseStoreContext ctx,
        IStoreClosureService service,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IClock clock)
    {
        _ctx = ctx;
        _service = service;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _clock = clock;
    }

    public async Task Handle(CloseStoreCommand command, CancellationToken ct)
    {
        _service.CloseStore(
            _ctx.Store,
            _ctx.Staff,
            _ctx.Catalog,
            _ctx.Assignments,
            _ctx.StoreCalendar,
            _ctx.StaffCalendars,
            _ctx.UpcomingAppointments,
            _clock
        );

        await _storeCalendarRepo.RemoveAsync(command.StoreId, ct);
        await _staffCalendarRepo.RemoveAsync(command.StoreId, ct);
    }
}