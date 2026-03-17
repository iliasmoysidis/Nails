using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffEmployeeLoader
    : IRequestContextLoader<
        RemoveStaffEmployeeCommand,
        RemoveStaffEmployeeContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public RemoveStaffEmployeeLoader(
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo)
    {
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
    }

    public async Task PopulateAsync(
        RemoveStaffEmployeeCommand command,
        RemoveStaffEmployeeContext ctx,
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