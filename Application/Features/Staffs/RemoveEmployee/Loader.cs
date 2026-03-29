using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.Staffs.RemoveEmployee;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public Loader(
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo)
    {
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException(
                $"Professional offerings not found for store {command.StoreId}.");

        ctx.Staff = staff;
        ctx.Assignments = assignments;
    }
}