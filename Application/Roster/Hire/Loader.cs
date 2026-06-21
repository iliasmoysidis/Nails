using Application.Professionals.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Schedule.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Professionals;
using Domain.Roster.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Roster.Hire;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalScheduleRepository _scheduleRepo;

    public Loader(
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
        Command command,
        Context ctx,
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
