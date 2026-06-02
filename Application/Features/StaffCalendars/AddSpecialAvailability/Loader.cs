using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Services;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IStaffRepository _staffRepo;

    public Loader(
        IStoreCalendarRepository storeCalendarRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IStaffRepository staffRepo)
    {
        _storeCalendarRepo = storeCalendarRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var professionalSchedule = await _professionalScheduleRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.ProfessionalAvailability = new ProfessionalAvailability(storeCalendar, professionalSchedule, command.StoreId);
        ctx.Staff = staff;
    }
}
