using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;

namespace Application.UseCases.StoreCalendar.SetDayOff;

public sealed class SetDayOffHandler : ICommandHandler<SetDayOffCommand>
{
    private readonly IStoreCalendarRepository _repo;
    private readonly StoreCalendarService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public SetDayOffHandler(IStoreCalendarRepository repo, StoreCalendarService service, ICurrentUser currentUser, IUnitOfWork uow)
    {
        _repo = repo;
        _service = service;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(SetDayOffCommand command, CancellationToken ct)
    {
        var calendar = await _repo.GetAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        _service.SetDayOff(calendar, staff, _currentUser.UserId, command.Day);

        await _uow.SaveChangesAsync(ct);
    }
}