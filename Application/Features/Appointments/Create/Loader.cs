using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Services;

namespace Application.Features.Appointments.Create;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IUserRepository _userRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public Loader(
        IUserRepository userRepo,
        IStoreRepository storeRepo,
        IStoreCatalogRepository storeCatalogRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IAssignmentsRepository assignmentsRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _userRepo = userRepo;
        _storeRepo = storeRepo;
        _storeCatalogRepo = storeCatalogRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _assignmentsRepo = assignmentsRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found");

        var storeCatalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found");

        var professionalAppointments = await _appointmentRepo.GetUpcomingByProfessionalIdAsync(command.ProfessionalId, ct);

        var userAppointments = await _appointmentRepo.GetByUserIdAsync(command.UserId, ct);

        ctx.User = user;

        ctx.AppointmentBooking = new AppointmentBooking(
            store: store,
            storeCalendar: storeCalendar,
            staffCalendar: staffCalendar,
            storeCatalog: storeCatalog,
            assignments: assignments,
            appointments: professionalAppointments
        );

        ctx.UserSchedule = new UserSchedule(
            userId: user.Id,
            appointments: userAppointments
        );
    }
}
