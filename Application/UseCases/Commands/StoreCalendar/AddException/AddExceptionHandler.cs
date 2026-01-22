using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;

namespace Application.UseCases.Commands.StoreCalendar.AddException;

public sealed class AddExceptionHandler : ICommandHandler<AddExceptionCommand>
{
    private readonly IStoreCalendarRepository _repo;
    private readonly StoreCalendarService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public AddExceptionHandler(IStoreCalendarRepository repo, StoreCalendarService service, ICurrentUser currentUser, IUnitOfWork uow)
    {
        _repo = repo;
        _service = service;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(AddExceptionCommand command, CancellationToken ct)
    {
        var calendar = await _repo.GetAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        _service.AddException(calendar, staff, _currentUser.UserId, command.Exception);

        await _uow.SaveChangesAsync(ct);
    }
}