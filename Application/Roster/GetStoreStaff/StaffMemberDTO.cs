namespace Application.Roster.GetStoreStaff;

public sealed record StaffMemberDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    string Phone,
    bool IsOwner,
    bool IsEmployee
);