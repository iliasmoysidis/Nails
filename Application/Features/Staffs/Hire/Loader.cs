using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Services;

namespace Application.Features.Staffs.Hire;

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
