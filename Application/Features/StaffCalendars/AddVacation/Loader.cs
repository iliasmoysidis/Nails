using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Services;

namespace Application.Features.StaffCalendars.AddVacation;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IStaffRepository _staffRepo;

    public Loader(
        IProfessionalRepository professionalRepo,
        IStoreRepository storeRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IStaffRepository staffRepo
    )
    {
        _professionalRepo = professionalRepo;
        _storeRepo = storeRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
        _staffRepo = staffRepo;
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

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var professionalSchedule = await _professionalScheduleRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.ProfessionalAvailability = new ProfessionalAvailability(
            professional: professional,
            store: store,
            storeCalendar: storeCalendar,
            professionalSchedule: professionalSchedule
        );

        ctx.Staff = staff;
    }
}
