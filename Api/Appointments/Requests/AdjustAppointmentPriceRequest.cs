namespace Api.Appointments.Requests;

public sealed record AdjustAppointmentPriceRequest(
    decimal Amount,
    string Currency,
    string Reason
);
