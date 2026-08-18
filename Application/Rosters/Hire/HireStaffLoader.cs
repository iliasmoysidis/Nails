using Application.Professionals.Common.Repositories;
using Application.Rosters.Common.Repositories;
using Application.Schedules.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Rosters;
using Domain.Professionals;
using Domain.Rosters.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Rosters.Hire;

public sealed class HireStaffLoader
    : IRequestContextLoader<
        HireStaffCommand,
        HireStaffContext>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalScheduleRepository _scheduleRepo;

    public HireStaffLoader(
        IStoreRepository storeRepo,
        IProfessionalRepository professionalRepo,
        IStaffRepository staffRepo,
        IProfessionalScheduleRepository scheduleRepo)
    {
        _storeRepo = storeRepo;
        _professionalRepo = professionalRepo;
        _staffRepo = staffRepo;
        _scheduleRepo = scheduleRepo;
    }

    public async Task PopulateAsync(
        HireStaffCommand command,
        HireStaffContext ctx,
        CancellationToken ct)
    {
        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var professional = await _professionalRepo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var schedule = await _scheduleRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        ctx.Staff = staff;

        ctx.EmploymentCreation = new EmploymentCreation(
            store,
            professional,
            staff,
            schedule
        );
    }
}
