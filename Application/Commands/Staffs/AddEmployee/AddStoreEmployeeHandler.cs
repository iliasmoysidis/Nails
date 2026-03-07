using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Staffs;

public sealed class AddStoreEmployeeHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public AddStoreEmployeeHandler(
        AuthorizationGuard auth,
        IStaffRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(AddStoreEmployeeCommand command, CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        staff.AddEmployee(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}