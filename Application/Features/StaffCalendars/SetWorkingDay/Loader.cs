using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Services;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;

    public Loader(
        IProfessionalRepository professionalRepo,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IProfessionalScheduleRepository professionalScheduleRepo
    )
    {
        _professionalRepo = professionalRepo;
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var professional = await _professionalRepo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var professionalSchedule = await _professionalScheduleRepo
            .GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        ctx.Staff = staff;

        ctx.ProfessionalAvailability = new ProfessionalAvailability(
            professional: professional,
            store: store,
            storeCalendar: storeCalendar,
            professionalSchedule: professionalSchedule
        );
    }
}
