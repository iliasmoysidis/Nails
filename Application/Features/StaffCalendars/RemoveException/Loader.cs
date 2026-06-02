using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Services;

namespace Application.Features.StaffCalendars.RemoveException;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;

    public Loader(
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IProfessionalScheduleRepository professionalScheduleRepo)
    {
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var professionalSchedule = await _professionalScheduleRepo
            .GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        ctx.Staff = staff;

        ctx.ProfessionalAvailability = new ProfessionalAvailability(
            storeCalendar,
            professionalSchedule,
            command.StoreId
        );
    }
}
